<template>
  <div id="app">
    <v-main class="d-flex align-center">
      <transition name="fade">
        <div class="text-center" v-show="show">
          <v-progress-circular
            :size="50"
            :width="3"
            color="primary"
            indeterminate
          ></v-progress-circular>
        </div>
      </transition>
    </v-main>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from '@vue/composition-api';
import { useUser } from '@/services/auth';
import { useCategories } from '@/composable/form';
import { noop } from '@/composable/compat';
import { useHttp } from '@/services/http';

export default defineComponent({
  emits: ['loaded'],
  name: 'Startup',
  setup(props, { emit }) {
    const { user, refresh } = useUser();
    onMounted(async () => {
      try {
        await prepareStartup();
      } finally {
        emit('loaded');
      }
    });

    const http = useHttp();
    const categories = useCategories();

    async function prepareStartup() {
      if (user.isAuthenticated) {
        user.loading = true;
        await refresh().catch(noop);
        // tracking user. must called after refresh to correctly track if user logged in
        user.loading = false;
      }

      await tracking().catch(noop);
      await categories.fetch().catch(noop);
    }

    async function tracking() {
      await http.post('/tracking/logs');
      // rerun tracking after x sec
      setTimeout(() => http.post('/tracking/logs'),
        Number(process.env.VUE_APP_API_TRACKING_REFRESH || '1000') * 1000);
    }

    // only show loading if startup take long time
    const show = ref(false);
    setTimeout(() => {
      show.value = true;
    }, 300);

    return {
      show,
    };
  },
});
</script>

<style scoped>
.fade-enter-active {
  transition: all 3s ease;
}

.fade-enter {
  opacity: 0;
}
</style>
