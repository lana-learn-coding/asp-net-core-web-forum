import { RouteConfig } from 'vue-router';
import ForumIndex from '@/views/forum/ForumIndex.vue';
import ForumSearch from '@/views/forum/ForumSearch.vue';
import ForumThreads from '@/views/forum/ForumThreads.vue';
import ThreadPosts from '@/views/forum/ThreadPosts.vue';
import Common from '@/layouts/Common.vue';

const forum: RouteConfig = {
  path: '/',
  component: Common,
  children: [
    {
      path: '/forums',
      component: ForumIndex,
      name: 'Forums',
    },
    {
      path: '/forums/search',
      component: ForumSearch,
      name: 'Search',
    },
    {
      path: '/forums/:slug',
      component: ForumThreads,
      name: 'Forum',
      props: true,
    },
    {
      path: '/threads/:slug',
      component: ThreadPosts,
      name: 'Thread',
      props: true,
    },
  ],
};

export default forum;
