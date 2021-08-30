import { RouteConfig } from 'vue-router';
import Layout from '@/layouts/Layout.vue';
import ForumIndex from '@/views/forum/ForumIndex.vue';

const forum: RouteConfig = {
  path: '/',
  component: Layout,
  children: [
    {
      path: '/forums',
      component: ForumIndex,
      name: 'Forums',
    },
  ],
};

export default forum;
