using CSharp.Models;
using SkiaSharp;

namespace CSharp.Helpers
{
    public static class ChartGenerator
    {
        public static byte[] GeneratePieChart(List<TimeEntry> data)
        {
            const int width = 700, height = 600;
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            float centerX = width / 2;
            float centerY = height / 2;
            float radius = 250;

            int total = data.Sum(e => e.TimeWorked);
            float startAngle = 0;

            var rand = new Random();
            var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true
            };
            var textFont = new SKFont(SKTypeface.Default, 16);

            foreach (var entry in data)
            {
                float sweepAngle = (float)entry.TimeWorked / total * 360;
                string label = $"{entry.Employee ?? "NaN"} [{((float)entry.TimeWorked / total * 100):0.#}%]";

                var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = new SKColor(
                        (byte)rand.Next(100, 256),
                        (byte)rand.Next(100, 256),
                        (byte)rand.Next(100, 256))
                };

                // Crtaj deo pie charta
                using var path = new SKPath();
                path.MoveTo(centerX, centerY);
                path.ArcTo(
                    new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius),
                    startAngle,
                    sweepAngle,
                    false);
                path.Close();

                canvas.DrawPath(path, paint);

                // Izračunaj sredinu segmenta
                float midAngle = startAngle + sweepAngle / 2;
                double radians = Math.PI * midAngle / 180.0;
                float labelRadius = radius * 0.6f;

                float labelX = centerX + labelRadius * (float)Math.Cos(radians);
                float labelY = centerY + labelRadius * (float)Math.Sin(radians);

                canvas.Save();
                var textRotationAngle = midAngle < 90 || midAngle > 270 ? midAngle : midAngle + 180; 
                canvas.RotateDegrees(textRotationAngle, labelX, labelY);
                canvas.DrawText(label, labelX, labelY, SKTextAlign.Center, textFont, textPaint);
                canvas.Restore();
                startAngle += sweepAngle;
            }

            using var image = SKImage.FromBitmap(bitmap);
            using var dataBytes = image.Encode(SKEncodedImageFormat.Png, 100);
            return dataBytes.ToArray();
        }
    }
}
