using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class ModalAnimator
{
    public static async Task ShowModalAsync(Form owner, Form modal)
    {
        modal.StartPosition = FormStartPosition.Manual;

        // Final centered position
        int finalX = owner.Left + (owner.Width - modal.Width) / 2;
        int finalY = owner.Top + (owner.Height - modal.Height) / 2;

        // Start slightly lower
        int startY = finalY + 60;

        modal.Location = new Point(finalX, startY);
        modal.Opacity = 0;

        modal.Show(owner);

        int frames = 30;

        for (int i = 0; i <= frames; i++)
        {
            // Ease-out cubic (iOS-like smoothness)
            double t = (double)i / frames;
            double ease = 1 - Math.Pow(1 - t, 3);

            int currentY = startY - (int)((startY - finalY) * ease);

            modal.Location = new Point(finalX, currentY);
            modal.Opacity = ease;

            await Task.Delay(8);
        }

        modal.Location = new Point(finalX, finalY);
        modal.Opacity = 1;
    }
}