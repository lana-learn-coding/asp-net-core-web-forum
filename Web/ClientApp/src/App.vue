<template>
  <div id="app">
    <startup v-if="preload" @loaded="() => preload = false"></startup>
    <transition name="load">
      <component v-if="!preload" :is="$route.meta.layout || 'main'">
        <router-view></router-view>
        <the-app-alert></the-app-alert>
      </component>
    </transition>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import Startup from '@/views/Startup.vue';
import TheAppAlert from '@/components/app/TheAppAlert.vue';

export default defineComponent({
  name: 'App',
  components: { TheAppAlert, Startup },
  setup() {
    const preload = ref(true);
    return {
      preload,
      Startup,
    };
  },
});
</script>

<style scoped>
.load-enter-active {
  transition: all .5s ease;
}

.load-enter {
  opacity: 0;
}
</style>

<style lang="scss">
body, main, #app {
  min-height: 100vh;
}

.v-application a.text-link {
  &:hover {
    color: rgba(0, 0, 0, .6);
  }
}

.w-full {
  width: 100%;
}

.h-full {
  height: 100%;
}
</style>
