<template>
  <div id="app">
    <v-app-bar app flat>
      <v-container class="py-0 fill-height">
        <v-toolbar-title class="ml-1">Dr. Forum</v-toolbar-title>

        <v-spacer></v-spacer>

        <v-responsive>
          <v-text-field
            v-model="keyword"
            dense
            flat
            clearable
            hide-details
            @focus="focus = true"
            @blur="focus = false"
            :solo="focus"
            :solo-inverted="!focus"
            prepend-inner-icon="search"
            placeholder="Search for forums..."
            @change="search"
          >
          </v-text-field>
        </v-responsive>

        <v-spacer></v-spacer>

        <template v-for="link in navs">
          <v-menu
            v-if="link.links"
            :key="link.text"
            offset-y
            open-on-hover
            close-on-click
            bottom
          >
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                class="mr-2"
                v-bind="attrs"
                v-on="on"
                text
              >
                {{ link.text }}
              </v-btn>
            </template>
            <v-list>
              <v-list-item
                v-for="(item) in link.links"
                :key="item.text"
              >
                <v-list-item-title>{{ item.text }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

          <v-btn
            v-else
            class="mr-2"
            :key="link.text"
            :to="{ name: link.name }"
            text
            exact-path
          >
            {{ link.text }}
          </v-btn>
        </template>

        <v-btn
          v-if="!user.isAuthenticated"
          :to="{ name: 'Login' }"
          exact-path
          text
        >
          Login
        </v-btn>

        <div v-else>
          <v-menu
            bottom
            min-width="200px"
            rounded
            offset-y
          >
            <template v-slot:activator="{ on }">
              <v-btn
                class="ml-4"
                icon
                x-large
                v-on="on"
              >
                <v-avatar>
                  <v-img
                    :src="user.avatar || '@/assets/anon.png'"
                    lazy-src="@/assets/anon_thumbnail.png"
                    :alt="user.username"
                  ></v-img>
                </v-avatar>
              </v-btn>
            </template>
            <v-card>
              <v-list-item-content class="justify-center">
                <div class="mx-auto text-center">
                  <v-avatar class="mb-2">
                    <v-img
                      :src="user.avatar || '@/assets/anon.png'"
                      lazy-src="@/assets/anon_thumbnail.png"
                      :alt="user.username"
                    ></v-img>
                  </v-avatar>
                  <h3>{{ user.username }}</h3>
                  <p class="caption mt-1">
                    {{ user.email }}
                  </p>
                  <v-divider class="my-2"></v-divider>
                  <v-btn
                    depressed
                    dense
                    text
                    :to="{ name: 'Me' }"
                  >
                    Profiles
                  </v-btn>
                  <v-divider class="my-2"></v-divider>
                  <v-btn
                    depressed
                    dense
                    text
                    @click="logout"
                  >
                    Log out
                  </v-btn>
                  <template v-auth="'Admin'">
                    <v-divider class="my-2"></v-divider>
                    <v-btn
                      depressed
                      dense
                      text
                      :to="{ name: 'Admin' }"
                    >
                      Admin
                    </v-btn>
                  </template>
                </div>
              </v-list-item-content>
            </v-card>
          </v-menu>
        </div>
      </v-container>
    </v-app-bar>

    <v-main class="grey lighten-2">
      <v-container class="mt-2 mt-md-3 mt-lg-4 mt-xl-5">
        <app-breadcrumbs class="mb-2 mb-md-3 mb-lg-4"></app-breadcrumbs>
        <router-view></router-view>
      </v-container>
    </v-main>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, watch } from '@vue/composition-api';
import { useTitle } from '@vueuse/core';
import { useUser } from '@/services/auth';
import { useCategories } from '@/composable/form';
import AppBreadcrumbs from '@/components/app/AppBreadcrumbs.vue';
import { useRouter } from '@/composable/compat';
import { useMessage } from '@/composable/message';

export default defineComponent({
  name: 'Layout',
  components: { AppBreadcrumbs },
  setup() {
    useTitle('Dr. Forums');
    const focus = ref(false);
    const navs = ref<NavLink[]>([
      {
        text: 'Forums',
        name: 'Home',
      },
      {
        text: 'Portal',
        name: 'Portal',
      },
      {
        text: 'Search',
        name: 'Search',
      },
      {
        text: 'Topics',
        links: [],
      },
    ]);

    const { user, logout } = useUser();
    const keyword = ref('');

    const categories = useCategories();
    watch(categories.loading, (loading) => {
      if (loading) return;
      const topic = navs.value.find((nav) => nav.text === 'Topics');
      if (!topic) return;
      topic.links = categories.data.value.map((cat) => ({
        text: cat.name,
        name: 'Categories',
        slug: cat.slug,
      }));
      navs.value = [...navs.value];
    });

    const router = useRouter();
    const { notify } = useMessage();

    function search() {
      if (keyword.value.trim() === '') {
        notify({ text: 'Please enter something to search' });
        return;
      }
      router.push({ name: 'Search', query: { search: keyword.value } });
    }

    return {
      search,
      user,
      navs,
      keyword,
      focus,
      logout,
      items: [
        {
          text: 'Dashboard',
          disabled: false,
          href: 'breadcrumbs_dashboard',
        },
        {
          text: 'Link 1',
          disabled: false,
          href: 'breadcrumbs_link_1',
        },
        {
          text: 'Link 2',
          disabled: true,
          href: 'breadcrumbs_link_2',
        },
      ],
    };
  },
});

interface NavLink {
  text: string,
  name?: string,
  links?: { text: string, name: string, slug: string }[]
}
</script>
