using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Nsnu.DataAccess.Enumerations;
using Nsnu.MembershipServices;

namespace WebApplication.Utilities
{
    public class InterfaceUtils
    {
        public static List<ListItem> GetEnumerationListItems<T>(bool includePleaseSelect)
        {
            var values = Enum.GetValues(typeof(T)).Cast<int>();

            List<ListItem> l = new List<ListItem>();

            foreach (var v in values)
            {
                var id = v.ToString();
                var label = ResourceUtils.GetEnumLabel((T) (object) v);
                //l.Add(new ListItem(ResourceUtils.GetEnumLabel((T)(object)v), id));
                l.Add(new ListItem(label, id));
            }
            

            l.Sort((thisItem, otherItem) => thisItem.Text.CompareTo(otherItem.Text));

            if (includePleaseSelect)
            {
                l.Insert(0, new ListItem("Please Select", string.Empty));
            }

            return l;
        }

        //public Dictionary<string, string> GetAllCommitteePositions(Committee c)
        //{
        //    var bs = new MembershipBs();
        //    Dictionary<string, string> list = (from cp in bs.GetCommitteePositions(c)
        //                                       select cp).ToDictionary(o => o.id.ToString(), o => o.positionName);


        //    Dictionary<string, string> altList = (from cp in bs.GetCommitteeAlternatePositions(c)
        //                                          select cp).ToDictionary(o => (o.id + 100).ToString(), o => String.Format("{0} (Alternate)", o.positionName));

        //    foreach (var alt in altList)
        //    {
        //        list.Add(alt.Key, alt.Value);
        //    }

        //    return list;
        //}

        public static List<ListItem> GetBoardOfDirectorsPositionListItems()
        {
            var bs = new MembershipBs();

            List<ListItem> altList = new List<ListItem>();

            List<ListItem> list = (from cp in bs.GetCommitteePositions(Committee.BoardOfDirectors)
                                   select cp).Select(i => new ListItem(i.positionName, i.id.ToString())).ToList();

            List<ListItem> subList = (GetEnumerationListItems<BoardOfDirectorsPosition>(false)).ToList();
            
            //list = list.Concat(subList).OrderBy(i => i.Text).ToList();
            list = list.Concat(subList).ToList().OrderBy(i => i.Text).ToList(); 

            //true BOD alternates
            altList = (from cp in bs.GetCommitteeAlternatePositions(Committee.BoardOfDirectors)
                                      select cp).Select(i => new ListItem(String.Format("{0} (Alternate)", i.positionName), (i.id + 100).ToString())).OrderBy(i => i.Text).ToList();

            altList.Add(new ListItem(String.Format("{0} (Alternate)", ResourceUtils.GetEnumLabel(BoardOfDirectorsPosition.BursaryChairCentral)), (((int) BoardOfDirectorsPosition.BursaryChairCentral) + 100).ToString()));
            altList.Add(new ListItem(String.Format("{0} (Alternate)", ResourceUtils.GetEnumLabel(BoardOfDirectorsPosition.BursaryChairEastern)), (((int) BoardOfDirectorsPosition.BursaryChairEastern) + 100).ToString()));
            altList.Add(new ListItem(String.Format("{0} (Alternate)", ResourceUtils.GetEnumLabel(BoardOfDirectorsPosition.BursaryChairWestern)), (((int) BoardOfDirectorsPosition.BursaryChairWestern) + 100).ToString()));
            altList.Add(new ListItem(String.Format("{0} (Alternate)", ResourceUtils.GetEnumLabel(BoardOfDirectorsPosition.BursaryChairNorthern)), (((int) BoardOfDirectorsPosition.BursaryChairNorthern) + 100).ToString()));

            altList = altList.OrderBy(i => i.Text).ToList();

            list = list.Concat(altList).ToList();

            list.Insert(0, new ListItem("Please Select", string.Empty));

            return list;
            //return list.Concat(altList).ToList();
        }

        public static List<ListItem> GetCommitteePositionListItems(Committee c)
        {
            return GetCommitteePositionListItems(c, true);
        }

        public static List<ListItem> GetCommitteePositionListItems(Committee c, bool includePleaseSelect)
        {
            var bs = new MembershipBs();

            List<ListItem> list = (from cp in bs.GetCommitteePositions(c)
                                   select cp).Select(i => new ListItem(i.positionName, i.id.ToString())).OrderBy(i => i.Text).ToList();

            List<ListItem> altList = (from cp in bs.GetCommitteeAlternatePositions(c)
                                      select cp).Select(i => new ListItem(String.Format("{0} (Alternate)", i.positionName), (i.id + 100).ToString())).OrderBy(i => i.Text).ToList();

            if (includePleaseSelect)
            {
                list.Insert(0, new ListItem("Please Select", string.Empty));    
            }
            
            return list.Concat(altList).ToList();
        }

        public static List<ListItem> GetLocalTableOfficerPositionListItems()
        {
            var bs = new MembershipBs();

            List<ListItem> localList = GetEnumerationListItems<LocalPosition>(false);

            List<ListItem> toList = (GetEnumerationListItems<TableOfficerPosition>(false)).Select(li => new ListItem(li.Text, (int.Parse(li.Value) + 100).ToString())).ToList();

            return localList.Concat(toList).ToList();
        }
    }
}