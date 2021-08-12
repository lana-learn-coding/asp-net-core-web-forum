import { RouteConfig } from 'vue-router';
import Error from '@/layouts/Error.vue';
import NotFound from '@/views/error/NotFound.vue';
import Unauthorized from '@/views/error/Unauthorized.vue';

const error: RouteConfig = {
  path: '/errors',
  component: Error,
  children: [
    {
      path: '/',
      redirect: () => ({ name: 'NotFound' }),
      name: 'Error',
    },
    {
      path: '404',
      component: NotFound,
      name: 'NotFound',
    },
    {
      path: '401',
      component: Unauthorized,
      name: 'Unauthorized',
    },
  ],
};
export default error;
