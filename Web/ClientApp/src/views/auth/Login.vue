<template>
  <div class="mt-10 d-flex justify-center">
    <v-card style="width: 500px">
      <v-card-title>Login</v-card-title>
      <v-card-subtitle>Login to your account</v-card-subtitle>
      <v-card-text>
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
        <v-btn
          :disabled="user.loading"
          color="success"
          :to="{ name: 'SignUp'}"
          text
        >
          Sign Up
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from '@vue/composition-api';
import { Location, Route } from 'vue-router';
import { useTitle } from '@vueuse/core';
import { useForm } from '@/composable/form';
import { useRouter } from '@/composable/compat';
import { useUser } from '@/services/auth';
import { useBreadcrumbs } from '@/composable/breadcrumbs';

export default defineComponent({
  name: 'Login',
  props: {
    redirect: {
      type: Object as PropType<Location>,
      default: null,
    },
  },
  beforeRouteEnter(to: Route, from: Route, next) {
    next((vm) => {
      (vm as unknown as { redirect: Location }).redirect = from as Location;
    });
  },
  setup(props) {
    useTitle('Login');
    useBreadcrumbs([]);

    const { form, errors, setErrors } = useForm({
      username: '',
      password: '',
      rememberMe: true,
    });

    const { user, login } = useUser();

    const router = useRouter();

    async function submit() {
      try {
        await login(form);
        await router.push({ ...props.redirect } as Location);
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
