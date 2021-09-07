import { RouteConfig } from 'vue-router';
import Me from '@/views/user/Me.vue';
import User from '@/layouts/User.vue';

const user: RouteConfig = {
  path: '/me',
  component: User,
  children: [
    {
      path: '/',
      component: Me,
      name: 'Me',
    },
  ],
};

export default user;
