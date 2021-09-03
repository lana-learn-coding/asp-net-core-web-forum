<template>
  <v-app id="app">
    <startup v-if="preload" @loaded="() => preload = false"></startup>
    <transition name="load" v-if="!preload">
      <router-view></router-view>
    </transition>
    <the-app-alert></the-app-alert>
    <the-app-notify></the-app-notify>
  </v-app>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import Startup from '@/views/Startup.vue';
import TheAppAlert from '@/components/app/TheAppAlert.vue';
import TheAppNotify from '@/components/app/TheAppNotify.vue';

export default defineComponent({
  name: 'App',
  components: { TheAppNotify, TheAppAlert, Startup },
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
