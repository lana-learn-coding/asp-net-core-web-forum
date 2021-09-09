import { RouteConfig } from 'vue-router';
import Home from '@/views/Home.vue';
import Common from '@/layouts/Common.vue';
import UserIndex from '@/views/user/UserIndex.vue';

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
    {
      path: 'users',
      component: UserIndex,
      name: 'Users',
    },
  ],
};

export default home;
