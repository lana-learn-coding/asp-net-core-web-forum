import { ComponentInstance, getCurrentInstance } from '@vue/composition-api';
import VueRouter, { Route } from 'vue-router';

export function useVM(): ComponentInstance {
  const instance = getCurrentInstance();
  if (!instance?.proxy) throw new Error('Vue: instance proxy not exist');
  return instance.proxy;
}

export function useRoute(): Route {
  return useVM().$route;
}

export function useRouter(): VueRouter {
  return useVM().$router;
}
