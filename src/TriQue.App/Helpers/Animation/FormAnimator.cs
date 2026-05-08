using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriQue.Helpers.Animation
{
    /// <summary>
    /// iOS-style form transition — clean, instant, no fluff.
    /// Crossfade + subtle upward drift. That's it.
    /// </summary>
    public static class FormAnimator
    {
        private const int Steps = 28;
        private const int Delay = 7;
        private const int Drift = 12;

        public static async Task SwitchAsync(Form current, Form next, bool closeCurrentAfter = false)
        {
            if (current.IsDisposed || next.IsDisposed) return;

            next.Opacity = 0;
            next.StartPosition = FormStartPosition.Manual;
            next.Location = current.Location;
            next.Size = current.Size;
            next.Top += Drift;
            next.Show();

            var targetLocation = current.Location;

            for (int i = 0; i <= Steps; i++)
            {
                // Stop immediately if either form was disposed mid-animation
                if (current.IsDisposed || next.IsDisposed) return;

                double t = Ease((double)i / Steps);

                try
                {
                    current.Opacity = 1 - (t * t);
                    next.Opacity = t;
                    next.Top = targetLocation.Y + (int)(Drift * (1 - t));
                }
                catch (ObjectDisposedException)
                {
                    return;
                }

                await Task.Delay(Delay);
            }

            if (next.IsDisposed) return;

            try
            {
                next.Opacity = 1;
                next.Location = targetLocation;
            }
            catch (ObjectDisposedException) { return; }

            if (!current.IsDisposed)
            {
                try
                {
                    if (closeCurrentAfter)
                        current.Close();
                    else
                        current.Hide();

                    current.Opacity = 1;
                }
                catch (ObjectDisposedException) { }
            }
        }

        public static async Task OpenAsync(Form form)
        {
            if (form.IsDisposed) return;

            form.Opacity = 0;
            var target = form.Location;
            form.Top += Drift;
            form.Show();

            for (int i = 0; i <= Steps; i++)
            {
                if (form.IsDisposed) return;

                try
                {
                    double t = Ease((double)i / Steps);
                    form.Opacity = t;
                    form.Top = target.Y + (int)(Drift * (1 - t));
                }
                catch (ObjectDisposedException) { return; }

                await Task.Delay(Delay);
            }

            if (form.IsDisposed) return;

            try
            {
                form.Opacity = 1;
                form.Location = target;
            }
            catch (ObjectDisposedException) { }
        }

        private static double Ease(double t)
            => t < 0.5
                ? 2 * t * t
                : 1 - Math.Pow(-2 * t + 2, 2) / 2;
    }
}