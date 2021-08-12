import { RouteConfig } from 'vue-router';
import Admin from '@/layouts/Admin.vue';
import Dashboard from '@/views/admin/Dashboard.vue';

const admin: RouteConfig = {
  path: '/admin',
  component: Admin,
  children: [
    {
      path: '/',
      redirect: () => ({ name: 'DashBoard' }),
      name: 'Admin',
    },
    {
      path: 'dashboard',
      component: Dashboard,
      name: 'DashBoard',
    },
  ],
};

export default admin;
