import { RouteConfig } from 'vue-router';
import Blank from '@/layouts/Blank.vue';
import Error from '@/views/Error.vue';

const error: RouteConfig = {
  path: '/errors',
  component: Blank,
  children: [
    {
      path: '/',
      redirect: () => ({ name: 'NotFound' }),
      name: 'Error',
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
export default error;
