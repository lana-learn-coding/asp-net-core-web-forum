<template>
  <div class="mt-10 d-flex justify-center">
    <v-card style="width: 500px">
      <v-card-title>Login</v-card-title>
      <v-card-subtitle>Login to your account</v-card-subtitle>
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
          <v-checkbox v-model="form.rememberMe" label="Remember Me" dense></v-checkbox>
        </v-form>
      </v-card-text>

      <v-card-actions class="flex-column flex-sm-row pt-0 align-start align-sm-center">
        <div>
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
        </div>
        <router-link :to="{ name: 'Forgot' }" class="body-2 d-block ml-3">Forgot your password?
        </router-link>
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
    const { form, errors, setErrors } = useForm({
      username: '',
      password: '',
      rememberMe: true,
    });

    const { user, login } = useUser();
    if (user.isAuthenticated) back();

    const router = useRouter();

    async function submit() {
      try {
        await login(form);
        await back();
      } catch (e) {
        if (e.response?.data) {
          setErrors(e.response.data);
        }
      }
    }

    async function back() {
      if (props.redirect.name === 'SignUp') {
        await router.push({ name: 'Me' });
        return;
      }
      if (props.redirect.name === 'Logout') {
        try {
          await router.go(-2);
        } catch {
          await router.push({ name: 'Home' });
        }
        return;
      }
      await router.push({ ...props.redirect } as Location);
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
