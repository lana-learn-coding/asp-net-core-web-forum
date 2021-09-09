<template>
  <div class="mt-10 d-flex justify-center">
    <v-card style="width: 500px">
      <v-card-title>Login</v-card-title>
      <v-card-subtitle>Login to admin account</v-card-subtitle>
      <v-card-text class="pb-0">
        <v-form>
          <v-text-field
            v-model="form.username"
            :error-messages="errors.username"
            label="Username"
            persistent-placeholder
            required
          >
          </v-text-field>
          <v-text-field
            v-model="form.password"
            :error-messages="errors.password"
            label="Password"
            type="password"
            persistent-placeholder
            required
          >
          </v-text-field>
          <v-checkbox v-model="form.rememberMe" label="Remember Me" class="mt-2"></v-checkbox>
        </v-form>
      </v-card-text>

      <v-card-actions>
        <v-btn color="primary" @click="submit" :loading="user.loading" text>
          Login
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/composition-api';
import { Location } from 'vue-router';
import { useTitle } from '@vueuse/core';
import { useForm } from '@/composable/form';
import { useRouter } from '@/composable/compat';
import { JwtToken, useUser } from '@/services/auth';
import { useCoreHttp } from '@/services/http';

export default defineComponent({
  name: 'AdminLogin',
  setup() {
    useTitle('Admin Login');

    const { form, errors, setErrors } = useForm({
      username: '',
      password: '',
      rememberMe: true,
    });

    const { user } = useUser();
    const { client } = useCoreHttp();

    async function login(form: { username: string, password: string, rememberMe: boolean }) {
      user.loading = true;
      try {
        const res = await client.post<JwtToken>('auth/login/admin', form);
        Object.assign(user, res.data.user);
        client.defaults.headers.Authorization = `Bearer ${res.data.token}`;
        user.isAuthenticated = true;
      } finally {
        user.loading = false;
      }
    }

    const router = useRouter();

    async function submit() {
      try {
        await login(form);
        await router.push({ name: 'Admin' } as Location);
      } catch (e) {
        if (e.response?.data) {
          setErrors(e.response.data);
        }
      }
    }

    return {
      user,
      submit,
      form,
      errors,
    };
  },
});
</script>
