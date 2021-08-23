import { RouteConfig } from 'vue-router';
import Admin from '@/layouts/Admin.vue';
import Dashboard from '@/views/admin/Dashboard.vue';
import ManageCategory from '@/views/admin/topic/ManageCategory.vue';
import ManageTag from '@/views/admin/topic/ManageTag.vue';
import ManageForum from '@/views/admin/ManageForum.vue';

const admin: RouteConfig = {
  path: '/admin',
  component: Admin,
  children: [
    {
      path: '/',
      redirect: () => ({ name: 'Dashboard' }),
      name: 'Admin',
    },
    {
      path: 'dashboard',
      component: Dashboard,
      name: 'Dashboard',
    },
    {
      path: 'categories',
      component: ManageCategory,
      name: 'ManageCategory',
    },
    {
      path: 'tags',
      component: ManageTag,
      name: 'ManageTag',
    },
    {
      path: 'forums',
      component: ManageForum,
      name: 'ManageForum',
    },
  ],
};

export default admin;
