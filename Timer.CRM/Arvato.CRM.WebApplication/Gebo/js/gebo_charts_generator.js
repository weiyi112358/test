function CombinedChart(d1, d2, elem, label1, label2) {
    // Setup the placeholder reference
    for (var i = 0; i < d1.length; ++i) { d1[i][0] += 60 * 120 * 1000 };
    for (var i = 0; i < d2.length; ++i) { d2[i][0] += 60 * 120 * 1000 };

    var options = {
        series: {
            curvedLines: { active: true }
        },
        yaxes: [
            {
                min: 0,
                position: "left",
                labelWidth: 50
            },
            {
                min: 0,
                position: "right"
            }
        ],
        xaxis: {
            mode: "time",
            minTickSize: [1, "day"],
            autoscaleMargin: 0.10

        },
        grid: {
            hoverable: true
        },
        legend: { position: 'nw' },
        colors: ["#8cc7e0", "#3ca0ca"]
    };

    // Setup the flot chart
    fl_d_plot = $.plot(elem,
        [
            {
                data: d1,
                label: label1,
                bars: {
                    show: true,
                    barWidth: 60 * 360 * 1000,
                    align: "center",
                    fill: 1
                }
            },
            {
                data: d2,
                label: label2,
                curvedLines: {
                    active: true,
                    show: true,
                    lineWidth: 3
                },
                yaxis: 2,
                points: { show: true },
                stack: null
            }
        ], options);

    // Create a tooltip on our chart
    elem.qtip({
        prerender: true,
        content: 'Loading...', // Use a loading message primarily
        position: {
            viewport: $(window), // Keep it visible within the window if possible
            target: 'mouse', // Position it in relation to the mouse
            adjust: { x: 7 } // ...but adjust it a bit so it doesn't overlap it.
        },
        show: false, // We'll show it programatically, so no show event is needed
        style: {
            classes: 'ui-tooltip-shadow ui-tooltip-tipsy',
            tip: false // Remove the default tip.
        }
    });

    // Bind the plot hover
    elem.on('plothover', function (event, coords, item) {
        // Grab the API reference
        var self = $(this),
            api = $(this).qtip(),
            previousPoint, content,

        // Setup a visually pleasing rounding function
        round = function (x) { return Math.round(x * 1000) / 1000; };

        // If we weren't passed the item object, hide the tooltip and remove cached point data
        if (!item) {
            api.cache.point = false;
            return api.hide(event);
        }

        // Proceed only if the data point has changed
        previousPoint = api.cache.point;
        if (previousPoint !== item.seriesIndex) {
            // Update the cached point data
            api.cache.point = item.seriesIndex;

            // Setup new content
            content = item.series.label + ': ' + round(item.datapoint[1]);

            // Update the tooltip content
            api.set('content.text', content);

            // Make sure we don't get problems with animations
            api.elements.tooltip.stop(1, 1);

            // Show the tooltip, passing the coordinates
            api.show(coords);
        }
    });
}

function PieChart(elem, data, colors, labelName) {

    // Setup the flot chart using our data
    fl_a_plot = $.plot(elem, data,
        {
            label: labelName,
            series: {
                pie: {
                    show: true,
                    innerRadius: 0.5,
                    highlight: {
                        opacity: 0.2
                    }
                }
            },
            grid: {
                hoverable: true,
                clickable: true
            },
            colors: colors
        }
    );

    // Create a tooltip on our chart
    elem.qtip({
        prerender: true,
        content: 'Loading...', // Use a loading message primarily
        position: {
            viewport: $(window), // Keep it visible within the window if possible
            target: 'mouse', // Position it in relation to the mouse
            adjust: { x: 7 } // ...but adjust it a bit so it doesn't overlap it.
        },
        show: false, // We'll show it programatically, so no show event is needed
        style: {
            classes: 'ui-tooltip-shadow ui-tooltip-tipsy',
            tip: false // Remove the default tip.
        }
    });

    // Bind the plot hover
    elem.on('plothover', function (event, pos, obj) {

        // Grab the API reference
        var self = $(this),
            api = $(this).qtip(),
            previousPoint, content,

        // Setup a visually pleasing rounding function
        round = function (x) { return Math.round(x * 1000) / 1000; };

        // If we weren't passed the item object, hide the tooltip and remove cached point data
        if (!obj) {
            api.cache.point = false;
            return api.hide(event);
        }

        // Proceed only if the data point has changed
        previousPoint = api.cache.point;
        if (previousPoint !== obj.seriesIndex) {
            percent = parseFloat(obj.series.percent).toFixed(2);
            // Update the cached point data
            api.cache.point = obj.seriesIndex;
            // Setup new content
            content = obj.series.label + ' ( ' + percent + '% )';
            // Update the tooltip content
            api.set('content.text', content);
            // Make sure we don't get problems with animations
            //api.elements.tooltip.stop(1, 1);
            // Show the tooltip, passing the coordinates
            api.show(pos);
        }
    });
}

function OrderdBarChart(elem, ds) {
    var options = {
        grid: {
            hoverable: true
        },
        xaxis: {
            mode: "time",
            minTickSize: [1, "day"],
            autoscaleMargin: 0.10
        },
        colors: ["#b4dbeb", "#8cc7e0", "#64b4d5", "#3ca0ca", "#2d83a6", "#22637e", "#174356", "#0c242e"]
    };

    fl_c_plot = $.plot(elem, ds, options);

    // Create a tooltip on our chart
    elem.qtip({
        prerender: true,
        content: 'Loading...', // Use a loading message primarily
        position: {
            viewport: $(window), // Keep it visible within the window if possible
            target: 'mouse', // Position it in relation to the mouse
            adjust: { x: 7 } // ...but adjust it a bit so it doesn't overlap it.
        },
        show: false, // We'll show it programatically, so no show event is needed
        style: {
            classes: 'ui-tooltip-shadow ui-tooltip-tipsy',
            tip: false // Remove the default tip.
        }
    });

    // Bind the plot hover
    elem.on('plothover', function (event, coords, item) {
        // Grab the API reference
        var self = $(this),
            api = $(this).qtip(),
            previousPoint, content,

        // Setup a visually pleasing rounding function
        round = function (x) { return Math.round(x * 1000) / 1000; };

        // If we weren't passed the item object, hide the tooltip and remove cached point data
        if (!item) {
            api.cache.point = false;
            return api.hide(event);
        }

        // Proceed only if the data point has changed
        previousPoint = api.cache.point;
        if (previousPoint !== item.seriesIndex) {
            // Update the cached point data
            api.cache.point = item.seriesIndex;

            // Setup new content
            content = item.series.label + ': ' + round(item.datapoint[1]);

            // Update the tooltip content
            api.set('content.text', content);

            // Make sure we don't get problems with animations
            api.elements.tooltip.stop(1, 1);

            // Show the tooltip, passing the coordinates
            api.show(coords);
        }
    });
}