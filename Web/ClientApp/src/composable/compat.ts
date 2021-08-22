import { ComponentInstance, getCurrentInstance, UnwrapRef } from '@vue/composition-api';
import VueRouter, { Route } from 'vue-router';
import { Framework as Vuetify } from 'vuetify';

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

// eslint-disable-next-line @typescript-eslint/no-empty-function
export function noop(): void {
}

export function useSetters<T>(obj: UnwrapRef<T>): { [K in keyof T]: (val: K) => void } {
  const setters = {};
  Object.keys(obj as T).forEach((key) => {
    setters[key] = (val) => {
      obj[key] = val;
    };
  });
  return setters as { [K in keyof T]: (val: K) => void };
}

export function useVuetify(): Vuetify {
  return useVM().$vuetify;
}
