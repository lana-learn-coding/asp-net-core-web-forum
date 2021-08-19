import Vue from 'vue';
import { getDirective } from 'vue-debounce';

Vue.directive('debounce', getDirective('2', {
  listenTo: 'input',
  defaultTime: '380ms',
  fireOnEmpty: false,
  trim: true,
}));
