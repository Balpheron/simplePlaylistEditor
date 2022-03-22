using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlaylistEditor
{

    public static class Syntax
    {
        public static readonly string listHeader = "#EXTM3U"; //тег для заголовка плейлиста
        public static readonly string channelHeader = "#EXTINF:"; //тег для заголовка канала или трека
        public static readonly string logoTag = "tvg-logo="; //тег для логотипа
        public static readonly string groupTag = "group-title="; //тег для названия группы каналов
        public static readonly string altGroupTag = "#EXTGRP:"; // альтернативный тег группы каналов
        public const string defaultGroupName = "Новая категория";
        public const string newChannelName = "Новый канал";
        public const string newPlaylistName = "Новый плейлист";
        public static string GetStringBetween(string sourceString, string replaceTag)
        {
            string result = sourceString.Replace(replaceTag, "");
            result = result.Replace("\"", "");
            return result;
            
        }
    }


}
