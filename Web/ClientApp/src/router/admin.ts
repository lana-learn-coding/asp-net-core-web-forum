import { RouteConfig } from 'vue-router';
import Admin from '@/layouts/Admin.vue';
import Dashboard from '@/views/admin/Dashboard.vue';
import ManageCategory from '@/views/admin/topic/ManageCategory.vue';
import ManageTag from '@/views/admin/topic/ManageTag.vue';
import ManageForum from '@/views/admin/ManageForum.vue';
import ManageUser from '@/views/admin/user/ManageUser.vue';
import AdminLogin from '@/views/auth/AdminLogin.vue';
import ManageThread from '@/views/admin/ManageThread.vue';
import ManagePost from '@/views/admin/ManagePost.vue';
import ManageSpecialty from '@/views/admin/topic/ManageSpecialty.vue';
import ManageLanguage from '@/views/admin/topic/ManageLanguage.vue';
import ManageCountry from '@/views/admin/user/ManageCountry.vue';
import ManageCity from '@/views/admin/user/ManageCity.vue';
import ManageExperience from '@/views/admin/user/ManageExperience.vue';
import ManagePosition from '@/views/admin/user/ManagePosition.vue';

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
      path: 'languages',
      component: ManageLanguage,
      name: 'ManageLanguage',
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
    {
      path: 'regions/countries',
      component: ManageCountry,
      name: 'ManageCountry',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'regions/cities',
      component: ManageCity,
      name: 'ManageCity',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'exp/experiences',
      component: ManageExperience,
      name: 'ManageExperience',
      meta: {
        roles: ['Admin'],
      },
    },
    {
      path: 'exp/positions',
      component: ManagePosition,
      name: 'ManagePosition',
      meta: {
        roles: ['Admin'],
      },
    },
  ],
};

export default admin;
