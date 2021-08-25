<template>
  <v-app>
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
  </v-app>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from '@vue/composition-api';
import { useUser } from '@/services/auth';

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

    async function prepareStartup() {
      if (user.isAuthenticated) {
        user.loading = true;
        try {
          await refresh();
        } finally {
          user.loading = false;
        }
      }
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
