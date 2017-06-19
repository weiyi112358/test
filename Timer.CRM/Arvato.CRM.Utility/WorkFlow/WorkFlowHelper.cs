using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvato.CRM.Utility.WorkFlow
{
    public class WorkFlowHelper
    {
        #region private field
        private const int NODE_WIDTH_SIZE = 100;
        private const int NODE_HEIGHT_SIZE = 32;
        private const int NODE_VERTICAL_PADDING_SIZE = 32;
        private const int NODE_PADDING_SIZE = 10;
        private const int IMG_FONT_PADDING = 1;

        private const string IMG_BRANCH = "~/img/gCons/network.png";
        private const string IMG_COUPON = "~/img/gCons/tag.png";
        private const string IMG_EMAIL = "~/img/gCons/email.png";
        private const string IMG_OUTBOUND = "~/img/gCons/phone.png";
        private const string IMG_SMS = "~/img/gCons/chat-.png";
        private const string IMG_WAIT = "~/img/gCons/copy-item.png";
        private const string IMG_SURVEY = "~/img/gCons/tag.png";

        private static Dictionary<ActivityCategory, string> Dict = new Dictionary<ActivityCategory, string>
        {
           {ActivityCategory.Branch,"流程分支"},
           {ActivityCategory.Coupon,"优惠劵"},
           {ActivityCategory.Mail,"邮件沟通"},
           {ActivityCategory.OB,"外呼沟通"},
           {ActivityCategory.SMS,"短信沟通"},
           {ActivityCategory.Question,"调查问卷"},
           {ActivityCategory.Wait,"等待反馈"},
           {ActivityCategory.Normal,"普通模板"}
        };
        #endregion

        /// <summary>
        /// 绘画树枝
        /// </summary>
        /// <param name="node"></param>
        /// <param name="point"></param>
        /// <param name="treeWeight"></param>
        private void drawBranch(Graphics g, Pen pen, Point pPoint, Point point)
        {
            var ori = pen.EndCap;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            pPoint.Y += 2;
            point.Y -= 2;

            g.DrawLines(pen,
                new Point[]{
                    pPoint,
                    new Point(pPoint.X, pPoint.Y + NODE_VERTICAL_PADDING_SIZE * 1/3),
                    new Point(point.X, pPoint.Y + NODE_VERTICAL_PADDING_SIZE * 1/3),
                    point
               });
            pen.EndCap = ori;
        }

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g"></param>
        /// <param name="act"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        private void draw(Graphics g, Activity act, int offsetX = 0, int offsetY = 0)
        {
            if (act == null) return;
            Pen pen = new Pen(Color.Black, 1);
            Brush brush = new SolidBrush(Color.Black);
            Font font = SystemFonts.DefaultFont;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            var size = GetSize(act);
            var fixedX = offsetX + (size.Width - NODE_WIDTH_SIZE) / 2;
            //Border
            var borderRect = new Rectangle(
                fixedX,
                offsetY,
                NODE_WIDTH_SIZE,
                NODE_HEIGHT_SIZE);
            g.DrawRectangle(pen, borderRect);

            //Image
            var imgWidth = 40;
            var imgHeight = 30;
            var img = getIcon(act.Category);
            var imgLeft = (imgWidth - img.Width) / 2;
            var imgTop = (imgHeight - img.Height) / 2;
            g.DrawImage(img, fixedX + imgLeft, offsetY + imgTop);

            //Title
            var titleRect = new Rectangle(
                fixedX + imgWidth - imgLeft,
                offsetY,
                NODE_WIDTH_SIZE - imgWidth,
                NODE_HEIGHT_SIZE);
            g.DrawString(Dict[act.Category], font, brush, titleRect, format);

            //Branch Start Point
            var bPoint = new Point(fixedX + NODE_WIDTH_SIZE / 2, offsetY + NODE_HEIGHT_SIZE);

            offsetY += NODE_HEIGHT_SIZE + NODE_VERTICAL_PADDING_SIZE;
            int _offsetX = 0;
            foreach (var a in act.Children)
            {
                var ssize = GetSize(a);
                draw(g, a, offsetX + _offsetX, offsetY);
                drawBranch(g, pen, bPoint, new Point(offsetX + _offsetX + ssize.Width / 2, offsetY));
                _offsetX += ssize.Width - NODE_PADDING_SIZE;
            }
        }

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private Image getIcon(ActivityCategory category)
        {
            switch (category)
            {
                case ActivityCategory.Branch:
                    return Image.FromFile(Util.MapPath(IMG_BRANCH));
                case ActivityCategory.Coupon:
                    return Image.FromFile(Util.MapPath(IMG_COUPON));
                case ActivityCategory.Mail:
                    return Image.FromFile(Util.MapPath(IMG_EMAIL));
                case ActivityCategory.OB:
                    return Image.FromFile(Util.MapPath(IMG_OUTBOUND));
                case ActivityCategory.SMS:
                    return Image.FromFile(Util.MapPath(IMG_SMS));
                case ActivityCategory.Question:
                    return Image.FromFile(Util.MapPath(IMG_SURVEY));
                case ActivityCategory.Wait:
                default:
                    return Image.FromFile(Util.MapPath(IMG_WAIT));

            }
        }

        /// <summary>
        /// 绘制流程图
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public static Bitmap CreateImage(Activity act)
        {
            WorkFlowHelper helper = new WorkFlowHelper();
            Size size = helper.GetSize(act);
            Bitmap bitmap = new Bitmap(size.Width + 20, size.Height + 20);
            Graphics g = Graphics.FromImage(bitmap);
            helper.draw(g, act, 10, 10);
            g.Dispose();
            return bitmap;
        }

        /// <summary>
        /// 获取活动维度
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private Size GetDimensions(Activity act, int x = 1, int y = 1)
        {
            if (act == null)
            {
                return new Size(x, y);
            }
            else
            {
                var y1 = y + 1;
                x += act.Children.Count > 1 ? act.Children.Count - 1 : 0;
                foreach (var a in act.Children)
                {
                    var s = GetDimensions(a, x, y1);
                    x = s.Width;
                    y = y > s.Height ? y : s.Height;
                }

            }
            return new Size(x, y);
        }

        private Size GetSize(Activity act)
        {
            if (act == null) return new Size(0, 0);
            var dime = GetDimensions(act);
            var pixWidth = dime.Width * (NODE_WIDTH_SIZE + NODE_PADDING_SIZE) + NODE_PADDING_SIZE;
            var pixHeight = dime.Height * (NODE_HEIGHT_SIZE + NODE_VERTICAL_PADDING_SIZE) + NODE_VERTICAL_PADDING_SIZE;
            return new Size(pixWidth, pixHeight);

        }

    }
}
