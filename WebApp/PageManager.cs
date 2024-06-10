using Messaging.Responses.ViewModels;

namespace WebApp
{
    public static class PageManager
    {
        public static int DisucssionsItemLimit = 5;
        public static int CommentsItemLimit = 5;
        public static int TopicsItemLimit = 5;
        public static int UsersItemLimit = 5;
        //public static int? DiscussionsPageIndex = 1;

        public static List<DiscussionViewModel> GetDiscussions(List<DiscussionViewModel> list, int pageIndex)
        {
            if(pageIndex >= 0 && pageIndex <= DiscussionsCountPages(list.Count()))
                return list.Skip((pageIndex-1)*DisucssionsItemLimit).Take(5).ToList();
            else return list;
        }

        public static int DiscussionsCountPages(int itemCount)
        {
            int pageCount = itemCount / DisucssionsItemLimit;
            if (itemCount % DisucssionsItemLimit != 0) pageCount++;

            return pageCount;
        }

        public static List<CommentViewModel> GetComments(List<CommentViewModel> list, int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex <= CommentsCountPages(list.Count()))
                return list.Skip((pageIndex - 1) * CommentsItemLimit).Take(5).ToList();
            else return list;
        }

        public static int CommentsCountPages(int itemCount)
        {
            int pageCount = itemCount / CommentsItemLimit;
            if (itemCount % CommentsItemLimit != 0) pageCount++;

            return pageCount;
        }

        public static List<TopicViewModel> GetTopics(List<TopicViewModel> list, int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex <= TopicsCountPages(list.Count()))
                return list.Skip((pageIndex - 1) * TopicsItemLimit).Take(5).ToList();
            else return list;
        }

        public static int TopicsCountPages(int itemCount)
        {
            int pageCount = itemCount / TopicsItemLimit;
            if (itemCount % TopicsItemLimit != 0) pageCount++;

            return pageCount;
        }

        public static List<UserViewModel> GetUsers(List<UserViewModel> list, int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex <= UsersCountPages(list.Count()))
                return list.Skip((pageIndex - 1) * UsersItemLimit).Take(5).ToList();
            else return list;
        }

        public static int UsersCountPages(int itemCount)
        {
            int pageCount = itemCount / UsersItemLimit;
            if (itemCount % UsersItemLimit != 0) pageCount++;

            return pageCount;
        }
    }
}
