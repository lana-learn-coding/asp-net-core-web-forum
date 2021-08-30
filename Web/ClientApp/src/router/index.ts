import Vue from 'vue';
import VueRouter, { NavigationGuardNext, Route, RouteConfig } from 'vue-router';
import admin from '@/router/admin';
import error from '@/router/error';
import home from '@/router/home';
import user from '@/router/user';
import { useAlert } from '@/composable/message';
import { isAuthorized, useUser } from '@/services/auth';
import forum from '@/router/forum';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  admin,
  home,
  error,
  user,
  forum,
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
});

const { show } = useAlert();
const { user: currentUser } = useUser();
router.beforeEach((to: Route, from: Route, next: NavigationGuardNext) => {
  if (!to.meta?.roles) return next();

  if (!currentUser.isAuthenticated) {
    show({
      text: 'Please login to continue',
      cb: async (ok) => {
        if (ok) router.push({ name: 'Login' });
      },
    });
    return next(false);
  }
  if (!isAuthorized(to.meta.roles)) return next({ name: 'Unauthorized' });
  return next();
});

export default router;
