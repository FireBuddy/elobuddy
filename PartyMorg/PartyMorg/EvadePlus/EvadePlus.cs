using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;

namespace EvadePlus
{
    public class EvadePlus
    {
        #region Properties

        public int IssueOrderTickLimit
        {
            get { return 0 /*Game.Ping * 2*/; }
        }

        #endregion

        #region Vars

        public SkillshotDetector SkillshotDetector { get; private set; }

        public EvadeSkillshot[] Skillshots { get; private set; }
        public Geometry.Polygon[] Polygons { get; private set; }
        public List<Geometry.Polygon> ClippedPolygons { get; private set; }
        public Vector2 LastIssueOrderPos;

        private readonly Dictionary<EvadeSkillshot, Geometry.Polygon> _skillshotPolygonCache;

        private EvadeResult LastEvadeResult;
        private Text StatusText;
        private int EvadeIssurOrderTime;

        #endregion

        public EvadePlus(SkillshotDetector detector)
        {
            Skillshots = new EvadeSkillshot[] {};
            Polygons = new Geometry.Polygon[] {};
            ClippedPolygons = new List<Geometry.Polygon>();
            _skillshotPolygonCache = new Dictionary<EvadeSkillshot, Geometry.Polygon>();

            SkillshotDetector = detector;
            SkillshotDetector.OnSkillshotDetected += OnSkillshotDetected;

            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            Dash.OnDash += OnDash;
        }

        private void OnSkillshotDetected(EvadeSkillshot skillshot, bool isProcessSpell)
        {
            //TODO: update
            if (skillshot.ToPolygon().IsInside(Player.Instance))
            {
                LastEvadeResult = null;
            }
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name == "summonerflash")
            {
                LastEvadeResult = null;
            }
        }

        private void OnDash(Obj_AI_Base sender, Dash.DashEventArgs dashEventArgs)
        {
            if (!sender.IsMe || LastEvadeResult == null)
            {
                return;
            }

            LastEvadeResult = null;
            Player.IssueOrder(GameObjectOrder.MoveTo, LastIssueOrderPos.To3DWorld(), false);
        }

        public bool IsPointSafe(Vector2 point)
        {
            return !ClippedPolygons.Any(p => p.IsInside(point));
        }

        public bool IsPathSafe(Vector2[] path)
        {
            return IsPathSafeEx(path);

#pragma warning disable CS0162 // Unreachable code detected
            for (var i = 0; i < path.Length - 1; i++)
#pragma warning restore CS0162 // Unreachable code detected
            {
                var start = path[i];
                var end = path[i + 1];

                if (ClippedPolygons.Any(p => p.IsInside(end) || p.IsInside(start) || p.IsIntersectingWithLineSegment(start, end)))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsPathSafe(Vector3[] path)
        {
            return IsPathSafe(path.ToVector2());
        }

        public bool IsHeroInDanger(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            return !IsPointSafe(hero.ServerPosition.To2D());
        }

        public int GetTimeAvailable(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            var skillshots = Skillshots.Where(c => _skillshotPolygonCache[c].IsInside(hero.Position)).ToArray();

            if (!skillshots.Any())
            {
                return short.MaxValue;
            }

            var times =
                skillshots.Select(c => c.GetAvailableTime(hero.ServerPosition.To2D()))
                    .Where(t => t > 0)
                    .OrderByDescending(t => t);

            return times.Any() ? times.Last() : short.MaxValue;
        }

        public int GetDangerValue(AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;
            var skillshots = Skillshots.Where(c => _skillshotPolygonCache[c].IsInside(hero.Position)).ToArray();

            if (!skillshots.Any())
                return 0;

            var values = skillshots.Select(c => c.SpellData.DangerValue).OrderByDescending(t => t);
            return values.Any() ? values.First() : 0;
        }

        public bool IsPathSafeEx(Vector2[] path, AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;

            for (var i = 0; i < path.Length - 1; i++)
            {
                var start = path[i];
                var end = path[i + 1];

                foreach (var pair in _skillshotPolygonCache)
                {
                    var skillshot = pair.Key;
                    var polygon = pair.Value;

                    if (polygon.IsInside(start) && polygon.IsInside(end))
                    {
                        //var time1 = skillshot.GetAvailableTime(start);
                        var time2 = skillshot.GetAvailableTime(end);

                        if (hero.WalkingTime(start, end) >= time2 - Game.Ping)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var intersections =
                            polygon.GetIntersectionPointsWithLineSegment(start, end)
                                .Concat(new[] {start, end})
                                .ToList()
                                .GetSortedPath(start).ToArray();

                        for (var i2 = 0; i2 < intersections.Length - 1; i2++)
                        {
                            var point1 = intersections[i2];
                            var point2 = intersections[i2 + 1];

                            if (polygon.IsInside(point2) || polygon.IsInside(point1))
                            {
                                //var time1 = polygon.IsInside(point1) ? skillshot.GetAvailableTime(point1) : short.MaxValue;
                                var time2 = polygon.IsInside(point2) ? skillshot.GetAvailableTime(point2) : short.MaxValue;

                                if (hero.WalkingTime(point1, point2) >= time2 - Game.Ping)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool IsPathSafeEx(Vector2 end, AIHeroClient hero = null)
        {
             hero = hero ?? Player.Instance;

            return IsPathSafeEx(hero.GetPath(end.To3DWorld(), true).ToVector2(), hero);
        }

        public Vector2[] GetEvadePoints(Vector2 from, float moveRadius)
        {
            var polygons = ClippedPolygons.Where(p => p.IsInside(from)).ToArray();
            var segments = new List<Vector2[]>();

            foreach (var pol in polygons)
            {
                for (var i = 0; i < pol.Points.Count; i++)
                {
                    var start = pol.Points[i];
                    var end = i == pol.Points.Count - 1 ? pol.Points[0] : pol.Points[i + 1];

                    var intersections =
                        Utils.GetLineCircleIntersectionPoints(from, moveRadius, start, end)
                            .Where(p => p.IsInLineSegment(start, end))
                            .ToList();

                    if (intersections.Count == 0)
                    {
                        if (start.Distance(from, true) < moveRadius.Pow() && end.Distance(from, true) < moveRadius.Pow())
                        {
                            intersections = new[] {start, end}.ToList();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (intersections.Count == 1)
                    {
                        intersections.Add(from.Distance(start, true) > from.Distance(end, true) ? end : start);
                    }

                    segments.Add(intersections.ToArray());
                }
            }

            if (!segments.Any()) //not enough time
            {
                return new Vector2[] {};
            }

            const int maxdist = 3000;
            const int division = 10;
            var points = new List<Vector2>();

            foreach (var segment in segments)
            {
                var dist = segment[0].Distance(segment[1]);
                if (dist > maxdist)
                {
                    segment[0] = segment[0].Extend(segment[1], dist / 2 - maxdist / 2);
                    segment[1] = segment[1].Extend(segment[1], dist / 2 - maxdist / 2);
                    dist = maxdist;
                }

                var step = maxdist / division;
                var count = dist / step;

                for (var i = 0; i < count; i++)
                {
                    var point = segment[0].Extend(segment[1], i * step);
                   
                    if (!point.IsWall() && IsPathSafeEx(point) && Player.Instance.GetPath(point.To3DWorld(), true).Length <= 2)
                    {
                        points.Add(point);
                    }
                }
            }

            return points.ToArray();
        }

        public Vector2 GetClosestEvadePoint(Vector2 from)
        {
            var polygons = ClippedPolygons.Where(p => p.IsInside(from)).ToArray();

            var polPoints =
                polygons.Select(pol => pol.ToDetailedPolygon())
                    .SelectMany(pol => pol.Points)
                    .OrderByDescending(p => p.Distance(from, true));

            return !polPoints.Any() ? Vector2.Zero : polPoints.Last();
        }

        public bool IsHeroPathSafe(EvadeResult evade, Vector3[] desiredPath, AIHeroClient hero = null)
        {
            hero = hero ?? Player.Instance;

            var path = (desiredPath ?? hero.RealPath()).ToVector2();
            return IsPathSafeEx(path, hero);

#pragma warning disable CS0162 // Unreachable code detected
            var polygons = ClippedPolygons;
#pragma warning restore CS0162 // Unreachable code detected
            var points = new List<Vector2>();

            for (var i = 0; i < path.Length - 1; i++)
            {
                var start = path[i];
                var end = path[i + 1];

                foreach (var pol in polygons)
                {
                    var intersections = pol.GetIntersectionPointsWithLineSegment(start, end);
                    if (intersections.Length > 0 && !pol.IsInside(hero))
                    {
                        return false;
                    }

                    points.AddRange(intersections);
                }
            }

            if (points.Count == 1)
            {
                var walkTime = hero.WalkingTime(points[0]);
                return walkTime <= evade.TimeAvailable;
            }

            return false;
        }

        public bool MoveTo(Vector2 point, bool limit = true)
        {
            if (limit && EvadeIssurOrderTime + IssueOrderTickLimit >= Environment.TickCount)
            {
                return false;
            }

            EvadeIssurOrderTime = Environment.TickCount;
            Player.IssueOrder(GameObjectOrder.MoveTo, point.To3DWorld(), false);

            return true;
        }

        public bool MoveTo(Vector3 point, bool limit = true)
        {
            return MoveTo(point.To2D(), limit);
        }

        public class EvadeResult
        {
            private EvadePlus Evade;
            private int ExtraRange { get; set; }

            public int Time { get; set; }
            public Vector2 PlayerPos { get; set; }
            public Vector2 EvadePoint { get; set; }
            public Vector2 AnchorPoint { get; set; }
            public int TimeAvailable { get; set; }
            public int TotalTimeAvailable { get; set; }
            public bool EnoughTime { get; set; }

            public bool IsValid
            {
                get { return !EvadePoint.IsZero; }
            }

            public Vector3 WalkPoint
            {
                get
                {
                    var walkPoint = EvadePoint.Extend(PlayerPos, -80);
                    var newPoint = walkPoint.Extend(PlayerPos, -ExtraRange);
                   
                    if (Evade.IsPointSafe(newPoint))
                    {
                        return newPoint.To3DWorld();
                    }

                    return walkPoint.To3DWorld();
                }
            }

            public bool Expired(int time = 4000)
            {
                return Elapsed(time);
            }

            public bool Elapsed(int time)
            {
                return Elapsed() > time;
            }

            public int Elapsed()
            {
                return Environment.TickCount - Time;
            }
        }
    }
}