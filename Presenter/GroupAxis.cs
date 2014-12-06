using NLog;
using PVK.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVK.Control.Presenter
{
    class GroupAxis
    {

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        double? _leftLinkLength;
        double maxDistance = 1.8;
        
        public int NumberGroup;
        public double? LeftLinkLength
        {
            get { return _leftLinkLength; }
            set { _leftLinkLength = value; }
        }
        double? _rightLinkLength;

        public double? RightLinkLength
        {
            get { return _rightLinkLength; }
            set { _rightLinkLength = value; }
        }
        List<Axis> _listAxis = new List<Axis>();
        public List<Axis> ListAxis
        {
            get { return _listAxis; }
            set { _listAxis = value; }
        }

        public List<GroupAxis> FillGroupAxis(PVK_WEIGHING weighing)
        {

            try
            {
                List<GroupAxis> res = new List<GroupAxis>();
                GroupAxis lastGroup = new GroupAxis(){NumberGroup = 0, LeftLinkLength = null};
                Axis axis;
                lastGroup.ListAxis.Add(new Axis() { numberAxis = 0 ,LeftLinkLength = null , RightLinkLength = weighing.DistanceAxis[0]});
                res.Add(lastGroup);

                for(int i = 0; i < weighing.CountAxis; i++)
                {
                    if (weighing.DistanceAxis[i] < maxDistance)
                    {
                        var lastAxis = lastGroup.ListAxis.OrderBy(t => t.numberAxis).LastOrDefault();
                        lastAxis.RightLinkLength = LeftLinkLength = weighing.DistanceAxis[i];
                        axis = new Axis(){numberAxis = lastAxis.numberAxis + 1, LeftLinkLength = weighing.DistanceAxis[i]};
                        lastGroup.ListAxis.Add(axis);
                    }
                    else
                    {
                        lastGroup.RightLinkLength = weighing.DistanceAxis[i];
                        axis = new Axis() { numberAxis = 0 };
                        lastGroup = new GroupAxis() { NumberGroup = lastGroup.NumberGroup + 1, LeftLinkLength = weighing.DistanceAxis[i] };
                    }
                }
                return res;

            }
            catch (Exception ex)
            {

                _logger.TraceException(ex.Message,ex);
                return null;
            }
        }

    }

    class Axis
    {
        public int numberAxis;
        double? _leftLinkLength;
        public double? LeftLinkLength
        {
            get { return _leftLinkLength; }
            set { _leftLinkLength = value; }
        }
        double? _rightLinkLength;
        public double? RightLinkLength
        {
            get { return _rightLinkLength; }
            set { _rightLinkLength = value; }
        }

   }
}
