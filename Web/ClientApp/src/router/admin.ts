import { RouteConfig } from 'vue-router';
import Admin from '@/layouts/Admin.vue';
import Dashboard from '@/views/admin/Dashboard.vue';
import ManageCategory from '@/views/admin/topic/ManageCategory.vue';
import ManageTag from '@/views/admin/topic/ManageTag.vue';
import ManageForum from '@/views/admin/ManageForum.vue';
import ManageUser from '@/views/admin/ManageUser.vue';
import AdminLogin from '@/views/auth/AdminLogin.vue';
import ManageThread from '@/views/admin/ManageThread.vue';
import ManagePost from '@/views/admin/ManagePost.vue';
import ManageSpecialty from '@/views/admin/topic/ManageSpecialty.vue';

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
      path: 'specialties',
      component: ManageSpecialty,
      name: 'ManageSpecialty',
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
      path: 'threads',
      component: ManageThread,
      name: 'ManageThread',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'posts',
      component: ManagePost,
      name: 'ManagePost',
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
