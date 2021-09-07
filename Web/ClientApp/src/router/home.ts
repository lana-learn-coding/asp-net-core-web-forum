import { RouteConfig } from 'vue-router';
import Home from '@/views/Home.vue';
import Common from '@/layouts/Common.vue';

const home: RouteConfig = {
  path: '/',
  component: Common,
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
