<template>
  <div class="d-flex align-center flex-column justify-center" style="margin-top: 35vh">
    <transition name="fade">
      <div class="text-center" v-show="show">
        <v-progress-circular
          :size="50"
          :width="3"
          color="primary"
          indeterminate
        ></v-progress-circular>
        <div class="mt-3 text--secondary">Logging Out</div>
      </div>
    </transition>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useUser } from '@/services/auth';
import { useRouter } from '@/composable/compat';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'Logout',
  setup() {
    useTitle('Logout');
    const router = useRouter();
    const { logout } = useUser();
    const { notify } = useMessage();
    onMounted(async () => {
      try {
        await logout();
      } catch (e) {
        notify({ text: 'Please reload page', type: 'Warn' });
      } finally {
        await router.back();
      }
    });

    const show = ref(false);
    setTimeout(() => {
      show.value = true;
    }, 200);

    return {
      show,
    };
  },
});
</script>

<style scoped>
.fade-enter-active {
  transition: all 2s ease;
}

.fade-enter {
  opacity: 0;
}
</style>
