import { RouteConfig } from 'vue-router';
import Blank from '@/layouts/Blank.vue';
import Error from '@/views/Error.vue';
import Login from '@/views/auth/Login.vue';
import SignUp from '@/views/auth/SignUp.vue';
import Logout from '@/views/auth/Logout.vue';
import Profile from '@/views/user/Profile.vue';
import ConfirmEmail from '@/views/auth/ConfirmEmail.vue';

const shared: RouteConfig = {
  path: '/',
  component: Blank,
  children: [
    {
      path: '/users/:slug',
      component: Profile,
      name: 'Profile',
      props: true,
    },
    {
      path: '/login',
      component: Login,
      name: 'Login',
    },
    {
      path: '/logout',
      component: Logout,
      name: 'Logout',
    },
    {
      path: '/sign-up',
      component: SignUp,
      name: 'SignUp',
    },
    {
      path: '/confirm-email/:token',
      component: ConfirmEmail,
      name: 'ConfirmEmail',
      props: true,
    },
    {
      path: '404',
      component: Error,
      name: 'NotFound',
      props: {
        code: '404',
        text: 'Page not found',
      },
    },
    {
      path: '403',
      component: Error,
      name: 'Forbidden',
      props: {
        code: '403',
        text: 'You don\'t have permission to access this resource',
      },
    },
  ],
};
export default shared;
