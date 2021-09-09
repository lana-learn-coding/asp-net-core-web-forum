import { RouteConfig } from 'vue-router';
import Home from '@/views/Home.vue';
import Common from '@/layouts/Common.vue';
import UserIndex from '@/views/user/UserIndex.vue';
import AdvancedSearch from '@/views/forum/AdvancedSearch.vue';

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
    {
      path: 'search/advanced',
      component: AdvancedSearch,
      name: 'AdvancedSearch',
    },
  ],
};

export default home;
