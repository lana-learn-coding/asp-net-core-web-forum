import { RouteConfig } from 'vue-router';
import Admin from '@/layouts/Admin.vue';
import Dashboard from '@/views/admin/Dashboard.vue';
import ManageCategory from '@/views/admin/topic/ManageCategory.vue';
import ManageTag from '@/views/admin/topic/ManageTag.vue';
import ManageForum from '@/views/admin/ManageForum.vue';
import ManageUser from '@/views/admin/ManageUser.vue';
import AdminLogin from '@/views/auth/AdminLogin.vue';

const admin: RouteConfig = {
  path: '/admin',
  component: Admin,
  children: [
    {
      path: 'login',
      component: AdminLogin,
      name: 'AdminLogin',
    },
    {
      path: '/',
      redirect: () => ({ name: 'Dashboard' }),
      name: 'Admin',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'dashboard',
      component: Dashboard,
      name: 'Dashboard',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'categories',
      component: ManageCategory,
      name: 'ManageCategory',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'tags',
      component: ManageTag,
      name: 'ManageTag',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'forums',
      component: ManageForum,
      name: 'ManageForum',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'users',
      component: ManageUser,
      name: 'ManageUser',
      meta: {
        roles: ['Admin'],
      },
    },
  ],
};

export default admin;
