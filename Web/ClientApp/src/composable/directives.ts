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
Vue.directive('auth', {
  bind(el, binding) {
    if (!user.isAuthenticated && el.style) {
      el.style.display = 'none';
    }

    const role = binding.expression?.replace(/['"]/g, '') || '';
    if (!role || role === 'User') return;
    if (role && user.roles && user.roles.includes(role)) return;

    if (el.style) {
      el.style.display = 'none';
    }
  },
});
