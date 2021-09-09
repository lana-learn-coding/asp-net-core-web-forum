import { RouteConfig } from 'vue-router';
import Me from '@/views/user/Me.vue';
import User from '@/layouts/User.vue';
import MeThread from '@/views/user/MeThread.vue';
import MePost from '@/views/user/MePost.vue';

const user: RouteConfig = {
  path: '/me',
  component: User,
  children: [
    {
      path: '/',
      component: Me,
      name: 'Me',
      meta: {
        roles: ['User'],
      },
    },
    {
      path: 'posts',
      component: MePost,
      name: 'MePost',
      meta: {
        roles: ['User'],
      },
    },
    {
      path: 'threads',
      component: MeThread,
      name: 'MeThread',
      meta: {
        roles: ['User'],
      },
    },
  ],
};

export default user;
