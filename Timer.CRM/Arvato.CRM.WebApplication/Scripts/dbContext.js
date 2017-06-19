/*--------------------------------------------------------------------------
* DbContext for JavaScript
* ver 1.0.0.0 (2014-01-13 11:25:47)
*
* created and maintained by Leo <66193250@qq.com>
*--------------------------------------------------------------------------*/
var TemplateConfig = {}, Templates = {};
+function ($) {
    var storage = $.localStorage, jsFileRgx = /.+\/(.+)\.js/i, dataFormat = 'yyyy-MM-dd hh:mm:ss', _ = jQuery, k = '12345678';         //本地localStorage操作对象

    //数组转化为Enumerable对象
    var query = function (arr) {
        return Enumerable.from(arr);
    }

    //判断是不是数组
    var isArray = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }

    //加载Js文件组
    var loadScripts = function (uris) {
        if (!isArray(uris)) return loadScripts([uris]);
        var ret = [];
        for (var i in uris) ret.push(loadScript(uris[i]));
        return _.when.apply(_, ret);
    }

    //加载Js文件
    var loadScript = function (uri) {
        var dtd = _.Deferred();
        var n = document.createElement('script');
        n.src = uri;// + "?_=" + Math.random();
        n.onload =
        n.onreadystatechange = function () {
            if (!this.readyState     //这是FF的判断语句，因为ff下没有readyState这人值，IE的readyState肯定有值
                || this.readyState == 'loaded'
                || this.readyState == 'complete'   // 这是IE的判断语句
                ) {
                dtd.resolve();
            }
        };
        n.onerror = function () {
            dtd.reject();
        }
        document.head.appendChild(n);
        return dtd.promise();
    }

    // 对Date的扩展，将 Date 转化为指定格式的String
    // 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
    // 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
    // 例子： 
    // (new Date()).format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
    // (new Date()).format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
    Date.prototype.format = function (fmt) { //author: meizz 
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3),  //季度  
            "S": this.getMilliseconds() //毫秒 
        };
        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    //本地数据对象
    var Entity = function () { }

    //新建一个对象
    //该对象不会马上存入本地数据中
    Entity.prototype.new = function (source) {
        source = source || {};
        var target = {}, template = this.template;
        for (var temp in template) {
            var teval = template[temp][0].toUpperCase(), toval = source[temp];
            switch (teval) {
                case 'AUTO': target[temp] = toval || that.identity; break;
                    //case 'GUID': target[temp] = toval || Guid.NewGuid(); break;
                case 'GETDATE': target[temp] = new Date().format(dataFormat); break;
                    //case 'DATETIME': target[temp] = toval; break;
                case 'BOOL': target[temp] = toval ? toval == true : toval; break;
                case 'LONG':
                case 'INT32':
                case 'INT64':
                case 'INT': target[temp] = toval ? parseInt(toval) : toval; break;
                case 'DECIMAL':
                case 'DOUBLE':
                case 'FLOAT': target[temp] = toval ? parseFloat(toval) : toval; break;
                default: target[temp] = toval; break;
            }
        }
        return target;
    }

    //主键筛选条件集合
    Entity.prototype.predicate = function (entity) {
        var predicate = this.keys();
        for (var p in predicate) {
            var func = Function('$', 'return ' + predicate[p]);
            predicate[p] += '[operator]\'' + func(entity) + '\'';
        }
        return predicate;
    }

    //获取对象主键
    //如果没有定义主键 系统默认第一个属性为主键
    Entity.prototype.keys = function () {
        var keys = [], config = TemplateConfig;
        if (config && config[this.entity] && config[this.entity]['Key']) {
            for (var key in config[this.entity]['Key']) {
                keys.push('$.' + config[this.entity]['Key'][key]);
            }
        }
        if (!keys.length) {
            for (var key in this.template) { keys.push('$.' + key); break; }
        }
        return keys;
    }

    //添加对象
    Entity.prototype.add = function (entity) {
        var target = this.new(entity), predicate = this.predicate(target), old, flag;
        old = this.selectOld(predicate.join(' && ').replace(/\[operator\]/g, ' == '));
        flag = old && old.any();
        //delete old;
        flag && Function('$', predicate.join(' , ').replace(/\[operator\]/g, ' = '))(target);

        target._chg_ = flag ? 1 : 0;//数据状态添加时为0，修改时为1，删除时为-1
        target._sync_ = 0;//同步状态初始状态为0,已同步为1
        target.ODA_Index = "001";

        var tempData = this.context.tempData && this.context.tempData[this.entity] || this.getStorage() || [];
        var q = query(tempData).where(predicate.join(' || ').replace(/\[operator\]/g, ' != '));
        tempData = q.toArray();
        //delete q;
        tempData.push(target);
        if (!flag) this.identity -= this.step;

        this.save(tempData);
        return target;
    };

    //修改对象
    //修改对象时请附带上该对象的主键
    Entity.prototype.edit = function (entity) {
        this.add(entity);
    }

    //删除指定条件的数据
    //可以直接传入指定对象，删除单个对象（请附带上该对象的主键）
    Entity.prototype.remove = function (predicate) {
        if (typeof predicate == 'object') {
            predicate = this.predicate(predicate).join(' && ').replace(/\[operator\]/g, '==');
            return this.remove(predicate);
        }
        var keys = this.keys().join('+'), tempData = [], a = Function("$", "return " + predicate + "&&$");
        var ienum = query(this.context.tempData && this.context.tempData[this.entity] || this.getStorage());
        this.selectOld(predicate).union(ienum, keys).forEach(function (x, i) {
            if (a(x)) {
                if (x._chg_ == 0) return;
                x._chg_ = -1, x._sync_ = 0;
            }
            tempData.push(x);
        });

        this.save(tempData);
    }

    //将数据保存到localStorage
    //事务开启的情况下 只会保存到内存中的临时数据中
    //内置函数 请不要调用
    Entity.prototype.save = function (da) {
        if (this.context.transactionStart) {
            this.context.tempData[this.entity] = da;
        } else {
            var d = this.context.decode[2];
            storage[d(this.entity, k)] = d(JSON.stringify(this.zip(da)), k);
            storage[d(this.entity + 'identity', k)] = this.identity;
        }
    }

    //数据压缩
    //将存在内存中的该类型的对象转化为数组对象
    //内置函数 请不要调用
    Entity.prototype.zip = function (d) {
        var l = [];
        for (var r in d) {
            var i = [];
            for (var c in d[r]) i.push(d[r][c]);
            l.push(i);
        }
        return [this.identity, l];
    }

    //数据解压缩
    //将数组对象转化为该实体类型的对象
    //内置函数 请不要调用
    Entity.prototype.unZip = function (s) {
        var t = [], e = this.entity, m = this.template;
        s = s || [1, []];
        this.identity = s[0];
        for (var r in s[1]) {
            var i = {}, x = 0;
            for (var col in m) i[col] = s[1][r][x++];
            i = this.new(i);
            i._chg_ = s[1][r][x] || 0;
            i._sync_ = s[1][r][x + 1] || 0;
            t.push(i);
        }
        return t;
    }

    //查找符合条件的数据集合（从内存和文件）
    Entity.prototype.select = function (predicate) {
        var keys = this.keys().join('+'), newData, oldData, data;
        newData = query(this.getStorage()).where(predicate);
        oldData = this.selectOld(predicate);
        return newData.union(oldData, keys).where('$._chg_ != -1');
    }

    //查找符合条件的数据集合（从文件）
    Entity.prototype.selectOld = function (predicate) {
        var oldData = query(), keys = this.keys().join('+'), decode = this.context.decode[2], config = TemplateConfig;

        if (config[this.entity] && config[this.entity].Source)
            for (var c in config[this.entity].Source) {
                d = decode(config[this.entity].Source[c].replace(jsFileRgx, '$1'), k);
                var f = new Function('return window["' + d.replace('"', '\\"') + '"]');
                var part = (f() || query()).where(predicate);
                oldData = oldData.union(part, keys)
            }
        return oldData;
    }

    //查找符合条件的第一个对象
    Entity.prototype.find = function (predicate) {
        var lst = this.select(predicate);
        return lst.firstOrDefault();
    }

    //从localStorage中读取数据
    Entity.prototype.getStorage = function () {
        var p = JSON.parse, d = this.context.decode[2], e = this.entity, s = storage[d(e, k)];
        return this.unZip(s && p(d(s, k)));
    }

    //清空本地数据
    Entity.prototype.clsStorage = function () {
        var d = this.context.decode[2], e = this.entity, s = storage[d(e, k)];
        storage.removeItem(d(e, k));
    }

    //加载Js文件并初始化数据
    //关联变量TemplateConfig
    Entity.prototype.load = function (callback) {
        var that = this, context = that.context, config = TemplateConfig;
        function l() {
            if (that.loaded !== 0) return;
            if (!navigator.onLine) {
                that.loaded = 1;
                context.ready(function () {
                    if (config && config[that.entity]) {
                        var uris = [], src = config[that.entity].Source || [], svc = config[that.entity].Service || [];
                        if (!isArray(svc)) svc = [svc];
                        for (var i in src) uris.push(src[i]);
                        for (var i in svc) uris.push(svc[i]);
                        loadScripts(uris).always(function () {
                            that.loaded = 2;
                            callback && callback.call(that);
                        })
                    } else {
                        that.loaded = 3;
                    }
                })
                //context.ready(function () {
                //    if (config && config[that.entity] && config[that.entity].Source && config[that.entity].Service) {
                //        var uris = [];
                //        for (var x in config[that.entity].Source) uris.push(config[that.entity].Source[x].replace(/\/\//g, '/'));
                //        config[that.entity].Service && loadScripts(config[that.entity].Service).done(function () {
                //            loadScripts(uris).always(function () {
                //                that.loaded = 2;
                //                callback && callback.call(that);
                //            })
                //        }).fail(function () { that.loaded = 3; })
                //    } else {
                //        that.loaded = 3;
                //    }
                //})
            }
        };
        _($).bind('offline', l), l();
    }

    var TimeOut = 100;

    //本地数据操作对象
    var DbContext = function () {
        //var that = this, def = {
        var that = this, d = that.decode[2], def = {
            loaded: 0,//DbContext加载状态 0：初始，1：加载中，2：加载完成，3：加载异常
            config: {
                globaljs: [['scripts/dbContext.Config.js'], ['scripts/dbContext.Config2.js', 'scripts/dbContext.Route.js'], ['scripts/dbContext.Source.js']]
            }
        };
        _.extend(that, def);

        if ($.globaljs) that.config.globaljs = $.globaljs;
        !function ls(idx) {
            that.loaded = 1;
            loadScripts(that.config.globaljs[idx]).done(function () {
                if (++idx >= that.config.globaljs.length) {
                    for (var template in Templates) {
                        var temp = that[template] = new Entity();
                        var ttemp = temp.template = Templates[template];
                        for (var t in ttemp) if (!isArray(ttemp[t])) ttemp[t] = [ttemp[t], 0]
                        temp.loaded = 0;//加载状态 0：初始，1：加载中，2：加载完成，3：加载异常
                        temp.entity = template;
                        //temp.identity = -1;
                        temp.identity = parseInt(storage[d(template + 'identity', k)] || '-1');
                        //console.log(temp.identity);
                        temp.step = 1;
                        temp.context = that;
                    }
                    loadScripts(that.store() + '/scripts/dbContext.Source.js').always(function () { that.loaded = 2 });
                } else ls(idx);
            }).fail(function () {
                that.loaded = 3;
            })
        }(0)
        return that;
    }

    //主要文件加载完成
    DbContext.prototype.ready = function (func) {
        var that = this;
        !function r() {
            that.loaded === 0 ? alert("dbContext 没有初始化") :
			that.loaded === 1 ? setTimeout(r, TimeOut) :
			that.loaded === 2 ? (func && func.call(that)) : alert("JavaScript 加载异常");
        }()
    }

    //Js离线数据版本号
    DbContext.prototype.DbVer = function (ver) {
        var key = '_dbver_', get = function () { return parseInt(localStorage[key] || 0) }, set = function (ver) { localStorage[key] = ver; };
        if (arguments.length > 0) { set(ver); return this; }
        return get();
    }

    //用户门店编号
    DbContext.prototype.store = function (stoid) {
        var key = '_dbstore_', get = function () { return localStorage[key] || 1000 }, set = function (stoid) { localStorage[key] = stoid; };
        if (arguments.length > 0) { set(stoid); return this; }
        return get();
    }

    //清理垃圾数据
    DbContext.prototype.reset = function () {
        var that = this, temps = Templates, config = TemplateConfig;
        if (config.dbver || 0 > that.DbVer()) {
            that.DbVer(config.dbver);
            for (var i in temps) {
                var ds = that[i].getStorage(), arr = query(ds).where('$._sync_==0').toArray();
                that[i].save(arr);
            }
        }
    }

    //数据同步
    DbContext.prototype.sync = function () {
        var that = this, temps = Templates, config = TemplateConfig;
        temps && function s() {
            var os = [];
            for (var i in temps) {
                if (!config[i].Sync) continue;
                var ds = that[i].getStorage(), tsync = query(ds).where('$._sync_==0'), sync = config[i].Sync, ssync = query(ds).where('$._sync_==1');
                if (tsync.any()) {
                    function succ(arr) {
                        for (var x in arr) {
                            arr[x]._sync_ = 1;
                            arr[x]._chg_ = arr[x]._chg_ || 1;
                            var predicate = that[i].predicate(arr[x]);
                            var tempData = that.tempData && that.tempData[that[i].entity] || that[i].getStorage() || [];
                            tempData = query(tempData).where(predicate.join(' || ').replace(/\[operator\]/g, ' != ')).toArray();
                            tempData.push(arr[x]);
                            that[i].save(tempData);
                        }
                        setTimeout(s, TimeOut);
                    };
                    function d(uri, status, query) {
                        var q = query.where('$._chg_ == ' + status);
                        if (q.any()) {
                            var d = q.toArray();
                            //console.log(JSON.stringify(d));
                            _.post(uri, g(i, d), function () { succ(d) });
                            return 1;
                        }
                        return 0;
                    };
                    function g(t, d) {
                        return 't=' + t + '&d=' + JSON.stringify(d).replace(/\"[\w_]+\":null,/g, '');                        
                        //var res = { t: t, l: d.length };
                        //for (var i in d) {
                        //    var r = d[i];
                        //    for (var k in r) res[i + '.' + k] = r[k];
                        //}
                        //return res;
                    }
                    if (sync.Del && d(sync.Del, -1, tsync) ||
                        sync.Add && d(sync.Add, 0, tsync) ||
                        sync.Edit && d(sync.Edit, 1, tsync)) return;
                } else if (ssync.any()) {
                    os.push(i + '|' + ssync.select('$.ODA_Index||"001"').distinct().toArray().join('.'));
                }
            }
            if (os.length > 0) _.post('/DataSync/Sync', { s: that.store(), os: os.join(",") });
        }()
    }

    //批量加载表数据
    //tables为dbContext的表，如[dbContext.Member,dbContext.Sale]
    DbContext.prototype.load = function (tables, callback) {
        var ret = [];
        for (var t in tables) {
            if (typeof tables[t] == 'string') tables[t] = this[tables[t]];
            tables[t] && tables[t].load && ret.push(tables[t].load());
        }
        _.when.apply(_, ret).done(callback);
    }

    //获取本地存储的数据
    DbContext.prototype.getStorage = function () {
        var d = {};
        for (var t in Templates) d[t] = this[t].getStorage();
        return d;
    }

    //清空本地存储的数据
    DbContext.prototype.clsStorage = function () {
        for (var t in Templates) this[t].clsStorage();
    }

    //数据加密函数集合
    DbContext.prototype.decode = [
        function (data, key) {
            if (key == null || key.length <= 0) {
                alert("Please enter a password with which to encrypt the message.");
                return null;
            }
            var prand = "";
            for (var i = 0; i < key.length; i++) {
                prand += key.charCodeAt(i).toString();
            }
            var sPos = Math.floor(prand.length / 5);
            var mult = parseInt(prand.charAt(sPos) + prand.charAt(sPos * 2) + prand.charAt(sPos * 3) + prand.charAt(sPos * 4) + prand.charAt(sPos * 5));
            var incr = Math.ceil(key.length / 2);
            var modu = Math.pow(2, 31) - 1;
            if (mult < 2) {
                alert("Algorithm cannot find a suitable hash. Please choose a different password.  Possible considerations are to choose a more complex or longer password.");
                return null;
            }
            var salt = Math.round(Math.random() * 1000000000) % 100000000;
            prand += salt;
            while (prand.length > 10) {
                prand = (parseInt(prand.substring(0, 10)) + parseInt(prand.substring(10, prand.length))).toString();
            }
            prand = (mult * prand + incr) % modu;
            var enc_chr = "";
            var enc_str = "";
            for (var i = 0; i < data.length; i++) {
                enc_chr = parseInt(data.charCodeAt(i) ^ Math.floor((prand / modu) * 255));
                if (enc_chr < 16) {
                    enc_str += "0" + enc_chr.toString(16);
                } else {
                    enc_str += enc_chr.toString(16);
                }
                prand = (mult * prand + incr) % modu;
            }
            salt = salt.toString(16);
            while (salt.length < 8) {
                salt = "0" + salt;
            }
            enc_str += salt;
            return enc_str;
        },
        function (data, key) {
            if (data == null || data.length < 8) {
                alert("A salt value could not be extracted from the encrypted message because it's length is too short. The message cannot be decrypted.");
                return;
            }
            if (key == null || key.length <= 0) {
                alert("Please enter a password with which to decrypt the message.");
                return;
            }
            var prand = "";
            for (var i = 0; i < key.length; i++) {
                prand += key.charCodeAt(i).toString();
            }
            var sPos = Math.floor(prand.length / 5);
            var mult = parseInt(prand.charAt(sPos) + prand.charAt(sPos * 2) + prand.charAt(sPos * 3) + prand.charAt(sPos * 4) + prand.charAt(sPos * 5));
            var incr = Math.round(key.length / 2);
            var modu = Math.pow(2, 31) - 1;
            var salt = parseInt(data.substring(data.length - 8, data.length), 16);
            data = data.substring(0, data.length - 8);
            prand += salt;
            while (prand.length > 10) {
                prand = (parseInt(prand.substring(0, 10)) + parseInt(prand.substring(10, prand.length))).toString();
            }
            prand = (mult * prand + incr) % modu;
            var enc_chr = "";
            var enc_str = "";
            for (var i = 0; i < data.length; i += 2) {
                enc_chr = parseInt(parseInt(data.substring(i, i + 2), 16) ^ Math.floor((prand / modu) * 255));
                enc_str += String.fromCharCode(enc_chr);
                prand = (mult * prand + incr) % modu;
            }
            return enc_str;
        },
        function (data, key) {
            var seq = [], x = 0, y = 0, l, das = [];
            if (isArray(key)) for (var k in key) seq[k] = key[k];
            else seq = getkey(key, 32);
            l = seq.length;
            for (var d in data) das[d] = data.charCodeAt(d);
            for (var d in das) {
                x = (x + 1) % l;
                y = (y + seq[x]) % l;
                var t = seq[x];
                seq[x] = seq[y];
                seq[y] = t;
                var rb = seq[(seq[x] + seq[y]) % l];
                das[d] = String.fromCharCode(das[d] ^ rb);
            }
            return das.join('');
        },
    ];

    function getkey(data, key) {
        var mbox = [], j = 0, t;
        for (var i = 0; i < key; i++) mbox[i] = i;
        for (var i = 0; i < key; i++) {
            j = (j + mbox[i] + data.charCodeAt(i % data.length)) % key;
            t = mbox[i];
            mbox[i] = mbox[j];
            mbox[j] = t;
        }
        return mbox;
    }

    //在事务中操作对象
    //该方法自动调用事务，函数调用完成后提交事务
    //如果要撤销操作，请抛出异常，系统收到异常后自动调用回滚函数，撤销当前修改
    //系统操作完成后，调用option对象的done方法，没有任何参数
    //系统操作失败后，调用option对象的fail方法，参数是Exception对象
    //建议使用该方法启动事务
    DbContext.prototype.transaction = function (func, option) {
        var that = this;
        that.beginTransaction();
        if (func && typeof func == 'function') {
            try {
                func.call(that);
                that.commit();
                option && option.succ && option.succ();
            } catch (ex) {
                that.rollback();
                option && option.fail && option.fail(ex);
            }
        }
    }

    //启动事务
    //启动事务后必须调用commit方法才能保存数据，否则数据变更将会被撤销
    DbContext.prototype.beginTransaction = function () {
        this.transactionStart = !0;
        this.tempData = {};
    }

    //提交事务中添加或修改的对象
    //该操作必须在事务中才生效
    DbContext.prototype.commit = function () {
        this.transactionStart = !1;
        if (this.tempData) {
            for (var temp in this.tempData) {
                this.save(temp)
            }
            this.tempData = null;
        }
    }

    //撤销事务中添加或修改的对象
    //该操作必须在事务中才生效
    DbContext.prototype.rollback = function () {
        this.transactionStart = null;
        this.tempData = null;
    }

    //保存指定对象集合到localStorage
    //entity 对象类名称，或几个对象类名称集合，如：'Product' 或 ['Product','User']
    //如果entity为空，将保存所有对象
    //尽量不要使用该方法，除非要强制刷新保存数据
    DbContext.prototype.save = function (e) {
        var d = this.decode[2], s = JSON.stringify, a = this.tempData;
        if (typeof e == 'string') return this.save([e]);
        if (!e) {
            e = []; for (var m in a) e.push(m);
        }
        if (isArray(e)) {
            for (var i in e) {
                storage[d(e[i], k)] = d(s(this[e[i]].zip(a[e[i]])), k);
                this.tempData[e[i]] = null;
            }
        }
    }

    $.dbContext = new DbContext();
}(window)

/*
* window.ajax for JavaScript
* ver 1.0.0.0 (2014-01-13 11:16:09)
*
* created and maintained by Leo <66193250@qq.com>
*--------------------------------------------------------------------------*/
+ function ($, _) {
    var ajax = function (uri, data, options) {
        if (typeof options == 'function') return ajax(uri, data, { callback: options });
        if (typeof options == 'bool') return ajax(uri, data, { async: options });
        if (typeof options == 'string') return ajax(uri, data, { dataType: options });
        dbContext.ready(function () {
            var cr, r = _.Route || [], o = {};
            o = $.extend({}, {
                cache: 0,
                //contentType: 'application/json',  //MVC 不能加这一句
                dataType: 'json',
                type: 'post',
                async: true
            }, options);
            for (var i in r) { if (r[i].Uri.toLowerCase() == uri.toLowerCase()) { cr = r[i]; break; } }
            if (!cr) return alert("你没有配置路由信息");
            if (cr.Req) {
                var ndata = $.extend({}, cr.Def || {}, data), errs = [];
                for (var i in cr.Req) {
                    if (ndata[cr.Req[i]] == undefined || ndata[cr.Req[i]] == "") { errs.push('字段 [' + cr.Req[i] + '] 不能为空'); }
                }
                if (errs.length) return alert(errs.join('\r'));
            }
            if (checkOnlineStatus()) ajaxLine(uri, data, o);
            else ajaxOff(cr.Fun, data, o)
        })
    },
	ready = function (callback) {
	    var t = Templates, r = 1;
	    for (var i in t) {
	        var d = dbContext[i].loaded;
	        if (d === 1) return setTimeout(function () { ready(callback) }, 100);
	        if (d !== 0 && d !== 2) { r = 1; break; }
	    }
	    if (r) callback && callback();
	},
	ajaxOff = function (func, data, o) {
	    ready(function () {
	        var _func = func;
	        if (func && typeof func == 'string') {
	            var fa = func.split('.'), co = '$', ca = [];
	            for (var i in fa) ca.push(co = co + '.' + fa[i]);
	            var anony = Function('$', 'return ' + ca.join('&&') + '&&' + func);
	            func = anony(_);
	        }
	        if (!func) {
	            //console.log(_func);
	            alert("该服务 不提供离线方法");
	        }
	        else {
	            var args = [], argsName, argsMatch = func
					.toString()
					.replace(/[\r|\n\ \t]/ig, '')
					.replace(/\/\*.+\*\//i, '')
					.match(/function\(([\w,\d]+)\)/i);
	            if (argsMatch) {
	                argsName = argsMatch[1].split(',');
	                for (var i in argsName) args.push(data[argsName[i]] != undefined ? data[argsName[i]] : data);
	            }
	            var res = func.apply(this, args);
	            o && o.callback && o.callback(res);
	        }
	    });
	},
	parseData = function (data, prex) {
	    var d = [], p = prex || '', n = /^\d+$/, p1 = p;
	    if (p1 != '') p1 += '.';
	    for (var td in data) {
	        if (n.test(td))
	            d.push(parseData(data[td], p + '[' + td + ']'));
	        else {
	            switch (Object.prototype.toString.call(data[td])) {
	                case '[object Array]': d.push(parseData(data[td], p1 + td)); break;
	                case '[object Object]': d.push(parseData(data[td], p1 + td)); break;
	                default: d.push((p1 + td + '=' + data[td]).replace(/&/ig, '%26')); break;//如果不对 请改成 %38
	            }
	        }
	    }
	    return d.join('&');
	},
	ajaxLine = function (uri, data, o) {
	    $.ajax({
	        cache: o.cache,
	        contentType: o.contentType,
	        type: o.type,
	        async: o.async,
	        url: uri,
	        dataType: o.dataType,
	        data: parseData(data),
	        success: function (result) { o.callback && o.callback(result); },
	        error: function (event, request, settings) { if (event.status == "500") { showCommonPromptPopup("网络状况不佳,请过30秒再试！"); } else { alert(event.statusText); } },
	        beforeSend: function () {
	            if (ajaxTime++ > 0) return;
	            ajaxLoading = $.loadBg("");
	        }, //添加loading信息
	        complete: function () {
	            if (--ajaxTime > 0) return;
	            if (ajaxLoading) { ajaxLoading.remove(); ajaxLoading = null; }
	        }    //清掉loading信息
	    });
	},
    ajaxLoading = null,
    ajaxTime = 0,
    isOnline = navigator.onLine,
    checkOnlineStatus = function () { return isOnline };
    $(function () {
        isOnline && dbContext.ready(function () { dbContext.reset(), dbContext.sync() });
        $(_).bind('online', function (e) {
            isOnline = true
            dbContext.ready(function () { dbContext.sync(); });
        }).bind("offline", function (e) {
            isOnline = false
        });
    });
    _.ajax = ajax;
}(jQuery, window)