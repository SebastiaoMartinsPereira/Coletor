using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TitaniumColector.Classes.Utility
{
    class ImageButton : System.Windows.Forms.Control
    {
        Image backGroudImage, pressedImage;

        bool pressed = false;

        public Image BackGroundImage 
        {
            get 
            {
                return this.backGroudImage;
            }
            set 
            {
                this.backGroudImage = value;
            }
        }

        // Property for the background image to be drawn behind the button text when
        // the button is pressed.
        public Image PressedImage
        {
            get
            {
                return this.pressedImage;
            }
            set
            {
                this.pressedImage = value;
            }
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            this.pressed = false;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (this.pressed && this.pressedImage != null)
                e.Graphics.DrawImage(this.pressedImage, 0, 0);
            else
                e.Graphics.DrawImage(this.backGroudImage, 0, 0);

            if (this.Text.Length > 0) 
            {
                SizeF size = e.Graphics.MeasureString(this.Text,this.Font);

                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor)
                    , (this.ClientSize.Width - size.Width) / 2
                    , (this.ClientSize.Height - size.Height) / 2);
            }
            e.Graphics.DrawRectangle(new Pen(Color.White), 0, 0
                , this.ClientSize.Width - 1
                , this.ClientSize.Height - 1);

            base.OnPaint(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.pressed = true;
            Rectangle rec = new Rectangle();
            System.Windows.Forms.PaintEventArgs paintEvent 
                = new System.Windows.Forms.PaintEventArgs(this.CreateGraphics(),rec);
            this.OnPaint(paintEvent);
            base.OnClick(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
        }
    }
}
