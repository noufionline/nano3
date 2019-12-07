﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Jasmine.Core.Controls.Emoji
{
    public static class EmojiData
    {
        public static EmojiTypeface Typeface { get; private set; }

        public static IEnumerable<Emoji> AllEmoji
        {
            get
            {
                foreach (var group in AllGroups)
                    foreach (var emoji in group.EmojiList)
                        yield return emoji;
            }
        }

        public static IEnumerable<Group> AllGroups { get; private set; }

        public static IDictionary<string, Emoji> Lookup { get; private set; }

        public static Regex MatchOne { get; private set; }
        public static Regex MatchMultiple { get; private set; }

        static EmojiData()
        {
            Typeface = new EmojiTypeface();
            ParseEmojiList();
        }

        public class Emoji
        {
            public string Name { get; set; }
            public string Text { get; set; }

            public Group Group => SubGroup.Group;
            public SubGroup SubGroup;

            public IList<Emoji> VariationList { get; } = new List<Emoji>();
        }

        public class SubGroup
        {
            public string Name { get; set; }
            public Group Group;

            public IList<Emoji> EmojiList { get; } = new List<Emoji>();
        }

        public class Group
        {
            public string Name { get; set; }
            public string Icon => SubGroups[0].EmojiList[0].Text;

            public IList<SubGroup> SubGroups { get; } = new List<SubGroup>();

            public int EmojiCount
            {
                get
                {
                    int i = 0;
                    foreach (var subgroup in SubGroups)
                        i += subgroup.EmojiList.Count;
                    return i;
                }
            }

            public IEnumerable<Emoji> EmojiList
            {
                get
                {
                    foreach (var subgroup in SubGroups)
                        foreach (var emoji in subgroup.EmojiList)
                            yield return emoji;
                }
            }
        }

        private static void ParseEmojiList()
        {
            var modifiers_list = new string[] { "🏻", "🏼", "🏽", "🏾", "🏿" };
            var modifiers_string = "(" + string.Join("|", modifiers_list) + ")";

            var match_group = new Regex(@"^# group: (.*)");
            var match_subgroup = new Regex(@"^# subgroup: (.*)");
            var match_sequence = new Regex(@"^([0-9a-fA-F ]+[0-9a-fA-F]).*; (fully-|minimally-|un)qualified.*# [^ ]* (.*)");
            var match_modifier = new Regex(modifiers_string);
            var list = new List<Group>();
            var lookup = new Dictionary<string, Emoji>();
            var alltext = new List<string>();

            Group last_group = null;
            SubGroup last_subgroup = null;
            Emoji last_emoji = null;

            foreach (var line in EmojiDescriptionLines())
            {
                var m = match_group.Match(line);
                if (m.Success)
                {
                    last_group = new Group() { Name = m.Groups[1].ToString() };
                    list.Add(last_group);
                    continue;
                }

                m = match_subgroup.Match(line);
                if (m.Success)
                {
                    last_subgroup = new SubGroup() { Name = m.Groups[1].ToString(), Group = last_group };
                    last_group.SubGroups.Add(last_subgroup);
                    continue;
                }

                m = match_sequence.Match(line);
                if (m.Success)
                {
                    string sequence = m.Groups[1].ToString();
                    string name = m.Groups[3].ToString();

                    string text = "";
                    foreach (var item in sequence.Split(' '))
                    {
                        int codepoint = Convert.ToInt32(item, 16);
                        text += char.ConvertFromUtf32(codepoint);
                    }

                    // Only include emojis that we know how to render
                    if (!Typeface.CanRender(text))
                        continue;

                    bool has_modifier = false;
                    bool has_high_modifier = false;
                    var regex_text = match_modifier.Replace(text, (x) =>
                    {
                        has_modifier = true;
                        has_high_modifier |= x.Value != modifiers_list[0];
                        return modifiers_string;
                    });

                    if (!has_high_modifier)
                        alltext.Add(has_modifier ? regex_text : text);

                    // Only add fully-qualified characters to the groups, or we will
                    // end with a lot of dupes.
                    if (line.Contains("unqualified") || line.Contains("minimally-qualified"))
                    {
                        // Skip this if there is already a fully qualified version
                        if (lookup.ContainsKey(text + "\ufe0f"))
                            continue;
                        if (lookup.ContainsKey(text.Replace("\u20e3", "\ufe0f\u20e3")))
                            continue;
                    }

                    var emoji = new Emoji() { Name = name, Text = text, SubGroup = last_subgroup };
                    lookup[text] = emoji;
                    if (has_modifier)
                    {
                        // We assume this is a variation of the previous emoji
                        if (last_emoji.VariationList.Count == 0)
                            last_emoji.VariationList.Add(last_emoji);
                        last_emoji.VariationList.Add(emoji);
                    }
                    else
                    {
                        last_emoji = emoji;
                        last_subgroup.EmojiList.Add(emoji);
                    }
                }
            }

            // Remove empty groups, for instance the Components
            for (int i = list.Count; --i > 0;)
                if (list[i].EmojiCount == 0)
                    list.RemoveAt(i);

            AllGroups = list;
            Lookup = lookup;

            // Build a regex that matches any Emoji
            var textarray = alltext.ToArray();
            Array.Sort(textarray, (a, b) => b.Length - a.Length);
            var regextext = "(" + string.Join("|", textarray).Replace("*", "[*]") + ")";
            MatchOne = new Regex(regextext);
            MatchMultiple = new Regex(regextext + "+");
        }

        private static IEnumerable<string> EmojiDescriptionLines()
        {
            var resource = typeof(EmojiData).Assembly.GetManifestResourceNames().Single(i => i.EndsWith("emojitest.txt")); 
            using (Stream s = typeof(EmojiData).Assembly.GetManifestResourceStream(resource))//Assembly.GetExecutingAssembly().GetManifestResourceStream("emoji-test.txt"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    foreach (var line in sr.ReadToEnd().Split('\r', '\n'))
                    {
                        yield return line;

                        // Hack to support those extra Microsoft emojis
                        if (line.EndsWith("🐱 cat face"))
                        {
                            yield return "1F431 200D 1F3CD ; fully-qualified # 🐱‍🏍 stunt cat";
                            yield return "1F431 200D 1F453 ; fully-qualified # 🐱‍👓 hipster cat";
                            yield return "1F431 200D 1F680 ; fully-qualified # 🐱‍🚀 astro cat";
                            yield return "1F431 200D 1F464 ; fully-qualified # 🐱‍👤 ninja cat";
                            yield return "1F431 200D 1F409 ; fully-qualified # 🐱‍🐉 dino cat";
                            yield return "1F431 200D 1F4BB ; fully-qualified # 🐱‍💻 hacker cat";
                        }
                    }
                }
            }
        }
    }
}
