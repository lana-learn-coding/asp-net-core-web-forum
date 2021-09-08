<template>
  <v-card flat>
    <v-card-title>Navigation</v-card-title>
    <v-card-text>
      <div class="d-flex justify-center py-3 py-lg-5 flex-column align-center">
        <v-skeleton-loader type="avatar" v-if="loading"></v-skeleton-loader>
        <v-avatar :size="$vuetify.breakpoint.mdAndDown ? 120 : 150" v-else>
          <v-img
            :src="user.avatar || require('@/assets/anon.png')"
            lazy-src="@/assets/anon_thumbnail.png"
            :alt="user.username"
          ></v-img>
        </v-avatar>
        <div class="mt-3 text--primary body-1 font-weight-medium">{{ user.username }}</div>
        <div>{{ user.email }}</div>
      </div>
      <v-divider></v-divider>
      <v-list
        dense
        nav
      >
        <v-list-item-group color="primary">
          <template v-for="link in navs">

            <v-list-item
              class="align-center"
              :key="link.text"
              :to="link.to"
              exact-path
              link
            >
              <v-list-item-icon>
                <v-icon class="material-icons-outlined">{{ link.icon }}</v-icon>
              </v-list-item-icon>

              <v-list-item-content>
                <v-list-item-title>{{ link.text }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>

          </template>

          <v-divider></v-divider>
          <v-list-item :to="{name: 'Admin'}" exact-path link v-auth="'Admin'">
            <v-list-item-icon>
              <v-icon class="material-icons-outlined">admin_panel_settings</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>Admin</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
          <v-list-item @click="logout">
            <v-list-item-icon>
              <v-icon class="material-icons-outlined">logout</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>Logout</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from '@vue/composition-api';
import { Dictionary } from '@/services/model';
import { useUser } from '@/services/auth';
import { useRouter } from '@/composable/compat';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'AppUserNavigation',
  setup() {
    const { user } = useUser();
    const loading = computed(() => false);

    return {
      ...useNavs(),
      loading,
      user,
    };
  },
});

function useNavs() {
  const navs = ref<NavLink[]>([
    {
      icon: 'home',
      text: 'Home',
      to: { name: 'Home' },
    },
    {
      icon: 'question_answer',
      text: 'My Threads',
      to: { name: 'MeThread' },
    },
    {
      icon: 'chat_bubble_outline',
      text: 'My Posts',
      to: { name: 'MePost' },
    },
    {
      icon: 'visibility',
      text: 'My Public Profile',
      to: { name: 'User' },
    },
  ]);

  const router = useRouter();
  const { confirm } = useMessage();

  async function logout() {
    const ok = await confirm({
      title: 'Logout',
      text: 'You are about to logout',
    });
    if (ok) await router.push({ name: 'Logout' });
  }

  return {
    navs,
    logout,
  };
}

interface NavLink {
  icon: string,
  text: string,
  to?: Dictionary,
}
</script>
