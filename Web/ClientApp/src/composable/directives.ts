import Vue from 'vue';
import { getDirective } from 'vue-debounce';
import { isAuthorized } from '@/services/auth';

Vue.directive('debounce', getDirective('2', {
  listenTo: 'input',
  defaultTime: '380ms',
  fireOnEmpty: false,
  trim: true,
}));

Vue.directive('auth', (el, binding) => {
  if (process.env.VUE_APP_OFFLINE_DISABLE_AUTH === 'true') return;
  const role = binding.expression?.replace(/['"]/g, '') || '';

  if (!isAuthorized(role)) {
    el.style.display = 'none';
  }
});
