using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace TriQue.Helpers
{
    public static class FontHelper
    {
        private static PrivateFontCollection fonts = new PrivateFontCollection();

        public static FontFamily RobotoFamily { get; private set; }
        public static Font RobotoRegular;
        public static Font RobotoBold;

        public static void Load()
        {
            string regularPath = Path.Combine(Application.StartupPath, "Assets", "Fonts", "Roboto-Regular.ttf");
            string boldPath = Path.Combine(Application.StartupPath, "Assets", "Fonts", "Roboto-Bold.ttf");

            if (!File.Exists(regularPath)) throw new FileNotFoundException("Missing: " + regularPath);
            if (!File.Exists(boldPath)) throw new FileNotFoundException("Missing: " + boldPath);

            fonts.AddFontFile(regularPath);
            fonts.AddFontFile(boldPath);

            RobotoFamily = fonts.Families[0];

            RobotoRegular = new Font(RobotoFamily, 10f, FontStyle.Regular);
            RobotoBold = new Font(RobotoFamily, 10f, FontStyle.Bold);
        }

        public static Font GetRoboto(float size, FontStyle style = FontStyle.Regular)
        {
            if (RobotoFamily == null)
            {
                return new Font("Arial", size, style);
            }
            return new Font(RobotoFamily, size, style);
        }
    }
}