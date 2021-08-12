import { RouteConfig } from 'vue-router';
import Layout from '@/layouts/Layout.vue';
import Home from '@/views/Home.vue';

const home: RouteConfig = {
  path: '/',
  component: Layout,
  children: [
    {
      path: '/',
      redirect: () => ({ name: 'Home' }),
    },
    {
      path: 'home',
      component: Home,
      name: 'Home',
    },
  ],
};

export default home;
