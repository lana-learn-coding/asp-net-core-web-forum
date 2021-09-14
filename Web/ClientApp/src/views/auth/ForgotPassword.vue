<template>
  <div class="mt-10 d-flex justify-center">
    <v-card style="width: 500px">
      <v-card-title>Forgot Password</v-card-title>
      <v-card-subtitle>Restore your password</v-card-subtitle>
      <v-card-text class="pb-0">
        <v-form>
          <v-text-field
            v-model="form.email"
            :error-messages="errors.email"
            label="Email"
            persistent-placeholder
            required
          >
          </v-text-field>
        </v-form>
      </v-card-text>

      <v-card-actions class="flex-column flex-sm-row pt-0 align-start align-sm-center">
        <div>
          <v-btn color="primary" @click="submit" :loading="loading" text>
            Submit
          </v-btn>
          <v-btn
            :disabled="loading"
            :to="{ name: 'Home'}"
            text
          >
            Home
          </v-btn>
        </div>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from '@vue/composition-api';
import { Location, Route } from 'vue-router';
import { useTitle } from '@vueuse/core';
import { useForm } from '@/composable/form';
import { noop, useRouter } from '@/composable/compat';
import { useHttp } from '@/services/http';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'ForgotPassword',
  beforeRouteEnter(to: Route, from: Route, next) {
    next((vm) => {
      (vm as unknown as { redirect: Location }).redirect = from as Location;
    });
  },
  setup() {
    useTitle('Forgot password');
    const { form, errors, setErrors } = useForm({
      email: '',
    });

    const loading = ref(false);
    const router = useRouter();
    const http = useHttp();
    const { confirm } = useMessage();

    async function submit() {
      loading.value = true;
      try {
        await http.post(`auth/forgot-password/${form.email}`);
        const ok = await confirm({
          title: 'Success',
          persistent: true,
          text: 'Please check your mailbox',
          ok: 'Home',
          cancel: 'Back',
        });
        if (ok) router.push({ name: 'Home' }).catch(noop);
        else router.back();
      } catch (e) {
        if (e.response?.data) {
          setErrors(e.response.data);
        }
      } finally {
        loading.value = false;
      }
    }

    return {
      loading,
      submit,
      form,
      errors,
    };
  },
});
</script>
