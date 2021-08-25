import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import admin from '@/router/admin';
import error from '@/router/error';
import home from '@/router/home';
import user from '@/router/user';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  admin,
  home,
  error,
  user,
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

export default router;
