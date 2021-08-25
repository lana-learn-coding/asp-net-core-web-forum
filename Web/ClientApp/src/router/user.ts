import { RouteConfig } from 'vue-router';
import Layout from '@/layouts/Layout.vue';
import Login from '@/views/auth/Login.vue';

const user: RouteConfig = {
  path: '/',
  component: Layout,
  children: [
    {
      path: '/login',
      component: Login,
      name: 'Login',
    }],
};

export default user;
