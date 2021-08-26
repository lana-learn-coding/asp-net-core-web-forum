import Vue from 'vue';
import { getDirective } from 'vue-debounce';
import { useUser } from '@/services/auth';

Vue.directive('debounce', getDirective('2', {
  listenTo: 'input',
  defaultTime: '380ms',
  fireOnEmpty: false,
  trim: true,
}));

const { user } = useUser();
Vue.directive('can', {
  bind(el, binding) {
    const role = binding.expression?.replace(/['"]/g, '') || '';
    if (user.roles.includes(role)) {
      return;
    }
    if (el.style?.display) {
      el.style.display = 'none';
    }
  },
});
