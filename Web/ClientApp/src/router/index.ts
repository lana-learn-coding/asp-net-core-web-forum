import Vue from 'vue';
import VueRouter, { NavigationGuardNext, Route, RouteConfig } from 'vue-router';
import admin from '@/router/admin';
import shared from '@/router/shared';
import home from '@/router/home';
import user from '@/router/user';
import { useAlert } from '@/composable/message';
import { isAuthorized, useUser } from '@/services/auth';
import forum from '@/router/forum';
import { noop } from '@/composable/compat';
import Error from '@/views/Error.vue';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  admin,
  home,
  shared,
  user,
  forum,
  {
    path: '*',
    component: Error,
    props: {
      code: '404',
      text: 'We can\'t find the page you\'re looking-for',
    },
  },
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

const { confirm } = useAlert();
const { user: currentUser } = useUser();
router.beforeEach((to: Route, from: Route, next: NavigationGuardNext) => {
  if (process.env.VUE_APP_OFFLINE_DISABLE_AUTH === 'true') return next();
  if (!to.meta?.roles) return next();

  if (!currentUser.isAuthenticated) {
    confirm({
      text: 'Please login to continue',
      persistent: true,
      cancel: 'Home',
      title: 'Login',
    })
      .then((ok) => {
        if (ok) router.push({ name: 'Login' }).catch(noop);
        else router.push({ name: 'Home' }).catch(noop);
      });
    return next(false);
  }
  if (!isAuthorized(to.meta.roles)) return next({ name: 'Forbidden' });
  return next();
});

export default router;
