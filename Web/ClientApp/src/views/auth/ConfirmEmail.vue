<template>
  <div class="d-flex align-center flex-column justify-center" style="margin-top: 32vh">
    <transition name="fade">
      <div class="text-center" v-if="show && loading">
        <v-progress-circular
          :size="50"
          :width="3"
          color="primary"
          indeterminate
        ></v-progress-circular>
        <div class="mt-3 text--secondary">Checking email</div>
      </div>
    </transition>
    <div v-if="!loading">
      <v-alert type="success" v-if="success">Your email have activated successfully</v-alert>
      <v-alert type="error" v-else>Invalid token, email activation failed</v-alert>
      <div class="d-flex justify-center">
        <v-btn text color="primary" :to="{ name: 'Home' }">Home</v-btn>
        <v-btn text v-if="user.isAuthenticated" :to="{ name: 'Me' }">Account</v-btn>
        <v-btn text v-else :to="{ name: 'Login' }">Login</v-btn>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import AutoCompleteSelect from '@/components/form/AutoCompleteSelect.vue';
import EditorInput from '@/components/form/EditorInput.vue';
import { useHttp } from '@/services/http';
import { useUser } from '@/services/auth';

export default defineComponent({
  name: 'ConfirmEmail',
  components: { EditorInput, AutoCompleteSelect },
  props: {
    token: String,
  },
  setup(props) {
    useTitle('Confirm Email');
    const loading = ref(true);
    const success = ref(true);
    const http = useHttp();

    onMounted(async () => {
      if (!props.token) {
        success.value = false;
        loading.value = false;
      }

      try {
        await http.post(`auth/confirm-email/${props.token}`);
        success.value = true;
        loading.value = false;
      } catch {
        success.value = false;
        loading.value = false;
      }
    });

    const show = ref(false);
    setTimeout(() => {
      show.value = true;
    }, 200);

    const { user } = useUser();
    return {
      user,
      show,
      loading,
      success,
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
