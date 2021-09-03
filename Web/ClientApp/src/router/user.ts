import { RouteConfig } from 'vue-router';
import Layout from '@/layouts/Layout.vue';
import Login from '@/views/auth/Login.vue';
import SignUp from '@/views/auth/SignUp.vue';

const user: RouteConfig = {
  path: '/',
  component: Layout,
  children: [
    {
      path: '/login',
      component: Login,
      name: 'Login',
    },
    {
      path: '/sign-up',
      component: SignUp,
      name: 'SignUp',
    },
  ],
};

export default user;
