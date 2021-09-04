import { RouteConfig } from 'vue-router';
import Layout from '@/layouts/Layout.vue';
import ForumIndex from '@/views/forum/ForumIndex.vue';
import ForumSearch from '@/views/forum/ForumSearch.vue';

const forum: RouteConfig = {
  path: '/',
  component: Layout,
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
  ],
};

export default forum;
