using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Typography.TextLayout;

namespace Jasmine.Core.Controls.Emoji
{
    public class EmojiTypeface
    {
        public EmojiTypeface()
            => m_fonts.Add(new ColorTypeface(null));

        public EmojiTypeface(string name)
            => m_fonts.Add(new ColorTypeface(name));

        public double Baseline
            => m_fonts[0].Baseline;

        public bool CanRender(string s)
            => m_fonts[0].CanRender(s);

        public double GetScale(double point_size)
            => m_fonts[0].GetScale(point_size);

        public GlyphPlanSequence MakeGlyphPlanSequence(string s)
        {
            if (!m_cache.TryGetValue(s, out var ret))
                m_cache[s] = ret = new GlyphPlanSequence(m_fonts[0].StringToGlyphPlanList(s));
            return ret;
        }

        public void RenderGlyph(DrawingContext dc, ushort gid, Point origin, double size, Brush fallback_brush)
            => m_fonts[0].RenderGlyph(dc, gid, origin, size, fallback_brush);

        /// <summary>
        /// A cache of GlyphPlanSequence objects, indexed by source strings. Should
        /// remain pretty lightweight because they are small objects.
        /// </summary>
        private IDictionary<string, GlyphPlanSequence> m_cache = new Dictionary<string, GlyphPlanSequence>();

        private IList<ColorTypeface> m_fonts = new List<ColorTypeface>();
    }
}
